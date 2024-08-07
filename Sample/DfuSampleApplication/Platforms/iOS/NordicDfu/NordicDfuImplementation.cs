using System;
using CoreBluetooth;
using Foundation;
using Plugin.NordicDfu.iOS;

namespace AndroidDfuSampleApplication;

    public class DfuImplementation
    {
        // public DFUFirmware selectedFirmware;
        public DFUServiceInitiator DFUInitiator;
        public DFUServiceController DFUController;

        public class DFUFirmwareLogger : LoggerDelegate
        {
            public override void Message(LogLevel level, string message)
            {
                System.Diagnostics.Debug.WriteLine(level.ToString() + ": " + message);

            }
        }

        public class DFUFirmwareDelegate : DFUServiceDelegate
        {
            INordicDfuCallbacks handler;
            public DFUFirmwareDelegate(INordicDfuCallbacks handler)
            {
                this.handler = handler;
            }
            public override void DfuError(DFUError error, string message)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("DfuError {0} {1}", error, message));
                handler?.OnError((int)error, 1, message);
            }

            public override void DfuStateDidChangeTo(DFUState state)
            {
                System.Diagnostics.Debug.WriteLine("DfuStateDidChangeTo " + state);
                if (state == DFUState.Completed) { handler?.OnDfuCompleted();  }
                else if (state == DFUState.Aborted) { handler?.OnDfuAborted(); }
                else if (state == DFUState.Validating) { handler?.OnFirmwareValidating(); }
                else
                {
                    //
                }
            }
        }

        public class DFUFirmwareProgress : DFUProgressDelegate
        {
            INordicDfuCallbacks handler;
            public DFUFirmwareProgress(INordicDfuCallbacks handler)
            {
                this.handler = handler;
            }
            public override void OutOf(nint part, nint totalParts, nint progress, double currentSpeedBytesPerSecond, double avgSpeedBytesPerSecond)
            {
                Console.Write("Part {0} of {1} ", part, totalParts);
                var percentInFirstPart = 100.0 / 3.0;
                var progressPercent = part < totalParts ? 
                    (progress * percentInFirstPart / 100.0) : 
                    (percentInFirstPart + (progress * (100.0 - percentInFirstPart) / 100.0));
                handler?.OnProgressChanged((int)Math.Round(progressPercent), (float)currentSpeedBytesPerSecond, (float) avgSpeedBytesPerSecond, (int)part, (int)totalParts);
            }
        }

        DFUFirmwareLogger logger;
        DFUFirmwareDelegate del;
        DFUFirmwareProgress prog;

        public DfuImplementation()
        {
        }

        
        public void Start(CBCentralManager cB, CBPeripheral device, byte[] firmwareZip, INordicDfuCallbacks callbackHandler)
        {
            var firmwareZipNSData = NSData.FromArray(firmwareZip);


            var selectedFirmware = new DFUFirmware(firmwareZipNSData,out NSError nSError);//, DFUFirmwareType.SoftdeviceBootloaderApplication);

            DFUServiceInitiator initiator = new DFUServiceInitiator(cB, device);
            initiator.WithFirmware(selectedFirmware);
            DFUInitiator = initiator;

            DFUInitiator.Logger = logger = new DFUFirmwareLogger();
            DFUInitiator.Delegate = del = new DFUFirmwareDelegate(callbackHandler);
            DFUInitiator.ProgressDelegate = prog = new DFUFirmwareProgress(callbackHandler);

            DFUController = DFUInitiator.Start(); //DFUInitiator.Start;
        }

        public void Start(CBCentralManager cB, CBPeripheral device, string firmwareZipPath, INordicDfuCallbacks callbackHandler) 
        {
            var firmwareZipNSData = NSData.FromFile(firmwareZipPath);
            var selectedFirmware = new DFUFirmware(firmwareZipNSData,out NSError nSError);

            
        }

        public void Stop()
        {
            DFUController.Abort();
            DFUController.Dispose();
        }
        public void Stop(CBCentralManager cB, CBPeripheral device)
        {
            DFUServiceInitiator initiator = new DFUServiceInitiator(cB, device);
            var dfuController = initiator.Start();
            dfuController.Abort();
            dfuController.Dispose();
        }


    }
