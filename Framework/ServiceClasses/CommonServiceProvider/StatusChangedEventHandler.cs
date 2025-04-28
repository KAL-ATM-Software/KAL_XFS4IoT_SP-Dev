/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Auxiliaries;
using XFS4IoT.CashManagement;
using XFS4IoT.Commands;
using XFS4IoT.Common;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass
    {
        private async Task StatusChangedEventHander(object sender, PropertyChangedEventArgs propertyInfo)
        {
            if (sender.GetType() == typeof(CommonStatusClass))
            {
                CommonStatusClass commonStatus = sender as CommonStatusClass;
                commonStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    Common: new(
                        Device: propertyInfo.PropertyName != nameof(commonStatus.Device) ?
                        null :
                        commonStatus.Device switch
                        {
                            CommonStatusClass.DeviceEnum.DeviceBusy => XFS4IoT.Common.DeviceEnum.DeviceBusy,
                            CommonStatusClass.DeviceEnum.FraudAttempt => XFS4IoT.Common.DeviceEnum.FraudAttempt,
                            CommonStatusClass.DeviceEnum.HardwareError => XFS4IoT.Common.DeviceEnum.HardwareError,
                            CommonStatusClass.DeviceEnum.NoDevice => XFS4IoT.Common.DeviceEnum.NoDevice,
                            CommonStatusClass.DeviceEnum.Offline => XFS4IoT.Common.DeviceEnum.Offline,
                            CommonStatusClass.DeviceEnum.Online => XFS4IoT.Common.DeviceEnum.Online,
                            CommonStatusClass.DeviceEnum.PotentialFraud => XFS4IoT.Common.DeviceEnum.PotentialFraud,
                            CommonStatusClass.DeviceEnum.PowerOff => XFS4IoT.Common.DeviceEnum.PowerOff,
                            CommonStatusClass.DeviceEnum.UserError => XFS4IoT.Common.DeviceEnum.UserError,
                            _ => throw new InternalErrorException($"Unexpected {nameof(commonStatus.Device)} property value specified. {commonStatus.Device}"),
                        },
                        DevicePosition: propertyInfo.PropertyName != nameof(commonStatus.DevicePosition) ?
                        null :
                        commonStatus.DevicePosition switch
                        {
                            CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                            CommonStatusClass.PositionStatusEnum.NotInPosition => XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                            CommonStatusClass.PositionStatusEnum.Unknown => XFS4IoT.Common.PositionStatusEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(commonStatus.DevicePosition)} property value specified. {commonStatus.DevicePosition}"),
                        },
                        PowerSaveRecoveryTime: propertyInfo.PropertyName != nameof(commonStatus.PowerSaveRecoveryTime) ?
                        null :
                        commonStatus.PowerSaveRecoveryTime,
                        AntiFraudModule: propertyInfo.PropertyName != nameof(commonStatus.AntiFraudModule) ?
                        null :
                        commonStatus.AntiFraudModule switch
                        {
                            CommonStatusClass.AntiFraudModuleEnum.DeviceDetected => XFS4IoT.Common.AntiFraudModuleEnum.DeviceDetected,
                            CommonStatusClass.AntiFraudModuleEnum.Inoperable => XFS4IoT.Common.AntiFraudModuleEnum.Inoperable,
                            CommonStatusClass.AntiFraudModuleEnum.Ok => XFS4IoT.Common.AntiFraudModuleEnum.Ok,
                            CommonStatusClass.AntiFraudModuleEnum.Unknown => XFS4IoT.Common.AntiFraudModuleEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(commonStatus.AntiFraudModule)} property value specified. {commonStatus.AntiFraudModule}"),
                        },
                        Exchange: propertyInfo.PropertyName != nameof(commonStatus.Exchange) ?
                        null :
                        commonStatus.Exchange switch
                        {
                            CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                            CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                            _ => throw new InternalErrorException($"Unexpected {nameof(commonStatus.Exchange)} property value specified. {commonStatus.Exchange}"),
                        })
                    ));
            }
            if (sender.GetType() == typeof(CardReaderStatusClass))
            {
                CardReaderStatusClass cardReaderStatus = sender as CardReaderStatusClass;
                cardReaderStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    CardReader: new(
                        Media: propertyInfo.PropertyName != nameof(cardReaderStatus.Media) ?
                        null :
                        cardReaderStatus.Media switch
                        {
                            CardReaderStatusClass.MediaEnum.Entering => XFS4IoT.CardReader.StatusClass.MediaEnum.Entering,
                            CardReaderStatusClass.MediaEnum.Jammed => XFS4IoT.CardReader.StatusClass.MediaEnum.Jammed,
                            CardReaderStatusClass.MediaEnum.Latched => XFS4IoT.CardReader.StatusClass.MediaEnum.Latched,
                            CardReaderStatusClass.MediaEnum.NotPresent => XFS4IoT.CardReader.StatusClass.MediaEnum.NotPresent,
                            CardReaderStatusClass.MediaEnum.Present => XFS4IoT.CardReader.StatusClass.MediaEnum.Present,
                            CardReaderStatusClass.MediaEnum.Unknown => XFS4IoT.CardReader.StatusClass.MediaEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.Media)} property value specified. {cardReaderStatus.Media}"),
                        },
                        Security: propertyInfo.PropertyName != nameof(cardReaderStatus.Security) ?
                        null :
                        cardReaderStatus.Security switch
                        {
                            CardReaderStatusClass.SecurityEnum.Open => XFS4IoT.CardReader.StatusClass.SecurityEnum.Open,
                            CardReaderStatusClass.SecurityEnum.NotReady => XFS4IoT.CardReader.StatusClass.SecurityEnum.NotReady,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.Security)} property value specified. {cardReaderStatus.Security}"),
                        },
                        ChipPower: propertyInfo.PropertyName != nameof(cardReaderStatus.ChipPower) ?
                        null :
                        cardReaderStatus.ChipPower switch
                        {
                            CardReaderStatusClass.ChipPowerEnum.Busy => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.Busy,
                            CardReaderStatusClass.ChipPowerEnum.HardwareError => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.HardwareError,
                            CardReaderStatusClass.ChipPowerEnum.NoCard => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.NoCard,
                            CardReaderStatusClass.ChipPowerEnum.NoDevice => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.NoDevice,
                            CardReaderStatusClass.ChipPowerEnum.Online => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.Online,
                            CardReaderStatusClass.ChipPowerEnum.PoweredOff => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.PoweredOff,
                            CardReaderStatusClass.ChipPowerEnum.Unknown => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.ChipPower)} property value specified. {cardReaderStatus.ChipPower}"),
                        },
                        ChipModule: propertyInfo.PropertyName != nameof(cardReaderStatus.ChipModule) ?
                        null :
                        cardReaderStatus.ChipModule switch
                        {
                            CardReaderStatusClass.ChipModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Inoperable,
                            CardReaderStatusClass.ChipModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Ok,
                            CardReaderStatusClass.ChipModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.ChipModule)} property value specified. {cardReaderStatus.ChipModule}"),
                        },
                        MagWriteModule: propertyInfo.PropertyName != nameof(cardReaderStatus.MagWriteModule) ?
                        null :
                        cardReaderStatus.MagWriteModule switch
                        {
                            CardReaderStatusClass.MagWriteModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Inoperable,
                            CardReaderStatusClass.MagWriteModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Ok,
                            CardReaderStatusClass.MagWriteModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.MagWriteModule)} property value specified. {cardReaderStatus.MagWriteModule}"),
                        },
                        FrontImageModule: propertyInfo.PropertyName != nameof(cardReaderStatus.FrontImageModule) ?
                        null :
                        cardReaderStatus.FrontImageModule switch
                        {
                            CardReaderStatusClass.FrontImageModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Inoperable,
                            CardReaderStatusClass.FrontImageModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Ok,
                            CardReaderStatusClass.FrontImageModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.FrontImageModule)} property value specified. {cardReaderStatus.FrontImageModule}"),
                        },
                        BackImageModule: propertyInfo.PropertyName != nameof(cardReaderStatus.BackImageModule) ?
                        null :
                        cardReaderStatus.BackImageModule switch
                        {
                            CardReaderStatusClass.BackImageModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Inoperable,
                            CardReaderStatusClass.BackImageModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Ok,
                            CardReaderStatusClass.BackImageModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(cardReaderStatus.BackImageModule)} property value specified. {cardReaderStatus.BackImageModule}"),
                        })
                        ));
            }
            if (sender.GetType() == typeof(BarcodeReaderStatusClass))
            {
                BarcodeReaderStatusClass barcodeReaderStatus = sender as BarcodeReaderStatusClass;
                barcodeReaderStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    BarcodeReader: new(
                        Scanner: propertyInfo.PropertyName != nameof(barcodeReaderStatus.ScannerStatus) ?
                        null :
                        barcodeReaderStatus.ScannerStatus switch
                        {
                            BarcodeReaderStatusClass.ScannerStatusEnum.Off => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.Off,
                            BarcodeReaderStatusClass.ScannerStatusEnum.On => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.On,
                            BarcodeReaderStatusClass.ScannerStatusEnum.Inoperative => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.Inoperative,
                            BarcodeReaderStatusClass.ScannerStatusEnum.Unknown => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected {nameof(barcodeReaderStatus.ScannerStatus)} property value specified. {barcodeReaderStatus.ScannerStatus}"),
                        }))
                    );
            }
            if (sender.GetType() == typeof(BiometricStatusClass))
            {
                BiometricStatusClass biometricStatus = sender as BiometricStatusClass;
                biometricStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    Biometric: new(
                        Subject: propertyInfo.PropertyName != nameof(biometricStatus.Subject) ?
                        null :
                        biometricStatus.Subject switch
                        {
                            BiometricStatusClass.SubjectStatusEnum.NotPresent => XFS4IoT.Biometric.StatusClass.SubjectEnum.NotPresent,
                            BiometricStatusClass.SubjectStatusEnum.Present => XFS4IoT.Biometric.StatusClass.SubjectEnum.Present,
                            BiometricStatusClass.SubjectStatusEnum.Unknown => XFS4IoT.Biometric.StatusClass.SubjectEnum.Unknown,
                            BiometricStatusClass.SubjectStatusEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unexpected {nameof(biometricStatus.Subject)} property value specified. {biometricStatus.Subject}"),
                        },
                        Capture: propertyInfo.PropertyName != nameof(biometricStatus.Capture) ?
                        null :
                        biometricStatus.Capture,
                        DataPersistence: propertyInfo.PropertyName != nameof(biometricStatus.DataPersistence) ?
                        null :
                        biometricStatus.DataPersistence switch
                        {
                            BiometricCapabilitiesClass.PersistenceModesEnum.Persist => XFS4IoT.Biometric.StatusClass.DataPersistenceEnum.Persist,
                            BiometricCapabilitiesClass.PersistenceModesEnum.Clear => XFS4IoT.Biometric.StatusClass.DataPersistenceEnum.Clear,
                            BiometricCapabilitiesClass.PersistenceModesEnum.None => null,
                            _ => throw new InternalErrorException($"Unexpected {nameof(biometricStatus.DataPersistence)} property value specified. {biometricStatus.DataPersistence}"),
                        },
                        RemainingStorage: propertyInfo.PropertyName != nameof(biometricStatus.RemainingStorage) ?
                        null :
                        biometricStatus.RemainingStorage
                        ))
                    );
            }
            if (sender.GetType() == typeof(VendorApplicationStatusClass))
            {
                VendorApplicationStatusClass vendorApplicationStatus = sender as VendorApplicationStatusClass;
                vendorApplicationStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    VendorApplication: new(
                        AccessLevel: propertyInfo.PropertyName != nameof(vendorApplicationStatus.AccessLevel) ?
                        null :
                        vendorApplicationStatus.AccessLevel switch
                        {
                            VendorApplicationStatusClass.AccessLevelEnum.Basic => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.Basic,
                            VendorApplicationStatusClass.AccessLevelEnum.NotActive => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.NotActive,
                            VendorApplicationStatusClass.AccessLevelEnum.Full => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.Full,
                            VendorApplicationStatusClass.AccessLevelEnum.Intermediate => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.Intermediate,
                            _ => throw new InternalErrorException($"Unexpected {nameof(vendorApplicationStatus.AccessLevel)} property value specified. {vendorApplicationStatus.AccessLevel}"),
                        }))
                    );
            }
            if (sender.GetType() == typeof(VendorModeStatusClass))
            {
                VendorModeStatusClass vendorModeStatus = sender as VendorModeStatusClass;
                vendorModeStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    VendorMode: new(
                        Device: propertyInfo.PropertyName != nameof(vendorModeStatus.DeviceStatus) ?
                        null :
                        vendorModeStatus.DeviceStatus switch
                        {
                            VendorModeStatusClass.DeviceStatusEnum.Offline => XFS4IoT.VendorMode.StatusClass.DeviceEnum.Offline,
                            VendorModeStatusClass.DeviceStatusEnum.Online => XFS4IoT.VendorMode.StatusClass.DeviceEnum.Online,
                            _ => throw new InternalErrorException($"Unexpected {nameof(vendorModeStatus.DeviceStatus)} property value specified. {vendorModeStatus.DeviceStatus}"),
                        },
                        Service: propertyInfo.PropertyName != nameof(vendorModeStatus.ServiceStatus) ?
                        null :
                        vendorModeStatus.ServiceStatus switch
                        {
                            VendorModeStatusClass.ServiceStatusEnum.EnterPending => XFS4IoT.VendorMode.StatusClass.ServiceEnum.EnterPending,
                            VendorModeStatusClass.ServiceStatusEnum.ExitPending => XFS4IoT.VendorMode.StatusClass.ServiceEnum.ExitPending,
                            VendorModeStatusClass.ServiceStatusEnum.Active => XFS4IoT.VendorMode.StatusClass.ServiceEnum.Active,
                            VendorModeStatusClass.ServiceStatusEnum.Inactive => XFS4IoT.VendorMode.StatusClass.ServiceEnum.Inactive,
                            _ => throw new InternalErrorException($"Unexpected {nameof(vendorModeStatus.ServiceStatus)} property value specified. {vendorModeStatus.ServiceStatus}"),
                        }))
                    );
            }
            if (sender.GetType() == typeof(TextTerminalStatusClass))
            {
                TextTerminalStatusClass textTerminalStatus = sender as TextTerminalStatusClass;
                textTerminalStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    TextTerminal: new(
                        Keyboard: propertyInfo.PropertyName != nameof(textTerminalStatus.Keyboard) ?
                        null :
                        textTerminalStatus.Keyboard switch
                        {
                            TextTerminalStatusClass.KeyboardEnum.Off => XFS4IoT.TextTerminal.StatusClass.KeyboardEnum.Off,
                            TextTerminalStatusClass.KeyboardEnum.On => XFS4IoT.TextTerminal.StatusClass.KeyboardEnum.On,
                            TextTerminalStatusClass.KeyboardEnum.NotAvailable => null,
                            _ => throw new InternalErrorException($"Unexpected {nameof(textTerminalStatus.Keyboard)} property value specified. {textTerminalStatus.Keyboard}"),
                        },
                        KeyLock: propertyInfo.PropertyName != nameof(textTerminalStatus.KeyLock) ?
                        null :
                        textTerminalStatus.KeyLock switch
                        {
                            TextTerminalStatusClass.KeyLockEnum.Off => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.Off,
                            TextTerminalStatusClass.KeyLockEnum.On => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.On,
                            TextTerminalStatusClass.KeyLockEnum.NotAvailable => null,
                            _ => throw new InternalErrorException($"Unexpected {nameof(textTerminalStatus.KeyLock)} property value specified. {textTerminalStatus.KeyLock}"),
                        },
                        DisplaySizeX: propertyInfo.PropertyName != nameof(textTerminalStatus.DisplaySizeX) ?
                        null :
                        textTerminalStatus.DisplaySizeX == -1 ? null : textTerminalStatus.DisplaySizeX,
                        DisplaySizeY: propertyInfo.PropertyName != nameof(textTerminalStatus.DisplaySizeY) ?
                        null :
                        textTerminalStatus.DisplaySizeY == -1 ? null : textTerminalStatus.DisplaySizeY))
                    );
            }
            if (sender.GetType() == typeof(KeyManagementStatusClass))
            {
                KeyManagementStatusClass keyManagementStatus = sender as KeyManagementStatusClass;
                keyManagementStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    KeyManagement: new(
                        EncryptionState: propertyInfo.PropertyName != nameof(keyManagementStatus.EncryptionState) ?
                        null :
                        keyManagementStatus.EncryptionState switch
                        {
                            KeyManagementStatusClass.EncryptionStateEnum.Ready => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Ready,
                            KeyManagementStatusClass.EncryptionStateEnum.Undefined => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Undefined,
                            KeyManagementStatusClass.EncryptionStateEnum.NotReady => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.NotReady,
                            KeyManagementStatusClass.EncryptionStateEnum.NotInitialized => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.NotInitialized,
                            KeyManagementStatusClass.EncryptionStateEnum.Initialized => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Initialized,
                            _ => throw new InternalErrorException($"Unexpected {nameof(keyManagementStatus.EncryptionState)} property value specified. {keyManagementStatus.EncryptionState}"),
                        },
                        CertificateState: propertyInfo.PropertyName != nameof(keyManagementStatus.CertificateState) ?
                        null :
                        keyManagementStatus.CertificateState switch
                        {
                            KeyManagementStatusClass.CertificateStateEnum.Unknown => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Unknown,
                            KeyManagementStatusClass.CertificateStateEnum.NotReady => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.NotReady,
                            KeyManagementStatusClass.CertificateStateEnum.Secondary => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Secondary,
                            KeyManagementStatusClass.CertificateStateEnum.Primary => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Primary,
                            KeyManagementStatusClass.CertificateStateEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unexpected {nameof(keyManagementStatus.CertificateState)} property value specified. {keyManagementStatus.CertificateState}"),
                        }))
                    );
            }
            if (sender.GetType() == typeof(KeyboardStatusClass))
            {
                KeyboardStatusClass keyboardStatus = sender as KeyboardStatusClass;
                keyboardStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                XFS4IoT.Keyboard.StatusClass.AutoBeepModeClass autoBeep = null;
                if (propertyInfo.PropertyName == nameof(keyboardStatus.AutoBeepMode) &&
                    keyboardStatus.AutoBeepMode != KeyboardStatusClass.AutoBeepModeEnum.NotSupported)
                {
                    autoBeep = new(
                        ActiveAvailable: keyboardStatus.AutoBeepMode.HasFlag(KeyboardStatusClass.AutoBeepModeEnum.Active),
                        InactiveAvailable: keyboardStatus.AutoBeepMode.HasFlag(KeyboardStatusClass.AutoBeepModeEnum.InActive)
                        );
                }

                await StatusChangedEvent(new(Keyboard: new(AutoBeepMode: autoBeep)));
            }
            if (sender.GetType() == typeof(CameraStatusClass.CameraLocationStatusClass))
            {
                CameraStatusClass.CameraLocationStatusClass cameraLocationStatus = sender as CameraStatusClass.CameraLocationStatusClass;
                cameraLocationStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                XFS4IoT.Camera.StatusClass.MediaClass mediaStatusLocation = null;
                XFS4IoT.Camera.StatusClass.CamerasClass camStatusLocation = null;
                XFS4IoT.Camera.StatusClass.PicturesClass numPicsLocation = null;

                if (propertyInfo.PropertyName == nameof(cameraLocationStatus.MediaState))
                {
                    XFS4IoT.Camera.MediaStateEnum newMediaState = cameraLocationStatus.MediaState switch
                    {
                        CameraStatusClass.CameraLocationStatusClass.MediaStateEnum.Ok => XFS4IoT.Camera.MediaStateEnum.Ok,
                        CameraStatusClass.CameraLocationStatusClass.MediaStateEnum.High => XFS4IoT.Camera.MediaStateEnum.High,
                        CameraStatusClass.CameraLocationStatusClass.MediaStateEnum.Full => XFS4IoT.Camera.MediaStateEnum.Full,
                        _ => XFS4IoT.Camera.MediaStateEnum.Unknown,
                    };

                    if (cameraLocationStatus.Location is not null &&
                        (cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room ||
                         cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot ||
                         cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person))
                    {
                        mediaStatusLocation = new(
                            Room: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room ?
                            newMediaState : null,
                            Person: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person ?
                            newMediaState : null,
                            ExitSlot: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot ?
                            newMediaState : null);

                    }
                    else if (!string.IsNullOrEmpty(cameraLocationStatus.CustomLocation))
                    {
                        mediaStatusLocation = new();
                        mediaStatusLocation.ExtendedProperties.Add(cameraLocationStatus.CustomLocation, newMediaState);
                    }
                }
                if (propertyInfo.PropertyName == nameof(cameraLocationStatus.CamerasState))
                {
                    XFS4IoT.Camera.CamerasStateEnum newCameraState = cameraLocationStatus.CamerasState switch
                    {
                        CameraStatusClass.CameraLocationStatusClass.CamerasStateEnum.Ok => XFS4IoT.Camera.CamerasStateEnum.Ok,
                        CameraStatusClass.CameraLocationStatusClass.CamerasStateEnum.Inoperable => XFS4IoT.Camera.CamerasStateEnum.Inop,
                        _ => XFS4IoT.Camera.CamerasStateEnum.Unknown,
                    };
                    if (cameraLocationStatus.Location is not null &&
                        (cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room ||
                         cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot ||
                         cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person))
                    {
                        camStatusLocation = new(
                            Room: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room ?
                            newCameraState : null,
                            Person: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person ?
                            newCameraState : null,
                            ExitSlot: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot ?
                            newCameraState : null);

                    }
                    else if (!string.IsNullOrEmpty(cameraLocationStatus.CustomLocation))
                    {
                        camStatusLocation = new();
                        camStatusLocation.ExtendedProperties.Add(cameraLocationStatus.CustomLocation, newCameraState);
                    }
                }
                if (propertyInfo.PropertyName == nameof(cameraLocationStatus.NumberOfPictures) &&
                    cameraLocationStatus.NumberOfPictures is not null)
                {
                    int newNumPicsLocation = (int)cameraLocationStatus.NumberOfPictures;
                    if (cameraLocationStatus.Location is not null &&
                        (cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room ||
                         cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot ||
                         cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person))
                    {
                        numPicsLocation = new(
                            Room: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room ?
                            newNumPicsLocation : null,
                            Person: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person ?
                            newNumPicsLocation : null,
                            ExitSlot: cameraLocationStatus.Location == CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot ?
                            newNumPicsLocation : null);

                    }
                    else if (!string.IsNullOrEmpty(cameraLocationStatus.CustomLocation))
                    {
                        numPicsLocation = new();
                        numPicsLocation.ExtendedProperties.Add(cameraLocationStatus.CustomLocation, newNumPicsLocation);
                    }
                }

                if (mediaStatusLocation is not null ||
                    camStatusLocation is not null ||
                    numPicsLocation is not null)
                {
                    await StatusChangedEvent(new(Camera: new(
                        Media: mediaStatusLocation,
                        Cameras: camStatusLocation,
                        Pictures: numPicsLocation)));
                }
            }
            if (sender.GetType() == typeof(CashManagementStatusClass))
            {
                CashManagementStatusClass cashManagementStatus = sender as CashManagementStatusClass;
                cashManagementStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    CashManagement: new(
                        Dispenser: propertyInfo.PropertyName != nameof(cashManagementStatus.Dispenser) ?
                        null :
                        cashManagementStatus.Dispenser switch
                        {
                            CashManagementStatusClass.DispenserEnum.Ok => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Ok,
                            CashManagementStatusClass.DispenserEnum.Unknown => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Unknown,
                            CashManagementStatusClass.DispenserEnum.Attention => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Attention,
                            CashManagementStatusClass.DispenserEnum.Stop => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Stop,
                            _ => null,
                        },
                        Acceptor: propertyInfo.PropertyName != nameof(cashManagementStatus.Acceptor) ?
                        null :
                        cashManagementStatus.Acceptor switch
                        {
                            CashManagementStatusClass.AcceptorEnum.Ok => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Ok,
                            CashManagementStatusClass.AcceptorEnum.Unknown => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Unknown,
                            CashManagementStatusClass.AcceptorEnum.Attention => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Attention,
                            CashManagementStatusClass.AcceptorEnum.Stop => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Stop,
                            _ => null,
                        }
                        )
                    ));
            }
            if (sender.GetType() == typeof(AuxiliariesStatusClass) ||
                sender.GetType() == typeof(AuxiliariesStatusClass.DoorStatusClass))
            {
                if (sender.GetType() == typeof(AuxiliariesStatusClass.DoorStatusClass))
                {
                    AuxiliariesStatusClass.DoorStatusClass doorStatus = sender as AuxiliariesStatusClass.DoorStatusClass;
                    doorStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (propertyInfo.PropertyName == nameof(doorStatus.DoorStatus))
                    {
                        await StatusChangedEvent(new(
                            Auxiliaries: new(
                                CabinetFrontDoor: doorStatus.Type != AuxiliariesCapabilitiesClass.DoorType.FrontCabinet ?
                                null :
                                doorStatus.DoorStatus switch
                                {
                                    AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Tampered,
                                    AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Open,
                                    AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Locked,
                                    AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Closed,
                                    AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Bolted,
                                    AuxiliariesStatusClass.DoorStatusEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected door status is specified. {doorStatus.Type} {doorStatus.DoorStatus}")
                                },
                                CabinetLeftDoor: doorStatus.Type != AuxiliariesCapabilitiesClass.DoorType.LeftCabinet ?
                                null :
                                doorStatus.DoorStatus switch
                                {
                                    AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Tampered,
                                    AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Open,
                                    AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Locked,
                                    AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Closed,
                                    AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Bolted,
                                    AuxiliariesStatusClass.DoorStatusEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected door status is specified. {doorStatus.Type} {doorStatus.DoorStatus}")
                                },
                                CabinetRearDoor: doorStatus.Type != AuxiliariesCapabilitiesClass.DoorType.RearCabinet ?
                                null :
                                doorStatus.DoorStatus switch
                                {
                                    AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Tampered,
                                    AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Open,
                                    AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Locked,
                                    AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Closed,
                                    AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Bolted,
                                    AuxiliariesStatusClass.DoorStatusEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected door status is specified. {doorStatus.Type} {doorStatus.DoorStatus}")
                                },
                                CabinetRightDoor: doorStatus.Type != AuxiliariesCapabilitiesClass.DoorType.RightCabinet ?
                                null :
                                doorStatus.DoorStatus switch
                                {
                                    AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Tampered,
                                    AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Open,
                                    AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Locked,
                                    AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Closed,
                                    AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Bolted,
                                    AuxiliariesStatusClass.DoorStatusEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected door status is specified. {doorStatus.Type} {doorStatus.DoorStatus}")
                                },
                                SafeDoor: doorStatus.Type != AuxiliariesCapabilitiesClass.DoorType.Safe ?
                                null :
                                doorStatus.DoorStatus switch
                                {
                                    AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Tampered,
                                    AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Open,
                                    AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Locked,
                                    AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Closed,
                                    AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Bolted,
                                    AuxiliariesStatusClass.DoorStatusEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected door status is specified. {doorStatus.Type} {doorStatus.DoorStatus}")
                                }))
                            );
                    }
                }
                else
                {
                    AuxiliariesStatusClass auxStatus = sender as AuxiliariesStatusClass;
                    auxStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        Auxiliaries: new(
                            OperatorSwitch: propertyInfo.PropertyName != nameof(auxStatus.OperatorSwitch) ?
                            null :
                            auxStatus.OperatorSwitch switch
                            {
                                AuxiliariesStatusClass.OperatorSwitchEnum.Run => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Run,
                                AuxiliariesStatusClass.OperatorSwitchEnum.Maintenance => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Maintenance,
                                AuxiliariesStatusClass.OperatorSwitchEnum.Supervisor => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Supervisor,
                                AuxiliariesStatusClass.OperatorSwitchEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.OperatorSwitch)} {auxStatus.OperatorSwitch}")
                            },
                            TamperSensor: propertyInfo.PropertyName != nameof(auxStatus.TamperSensor) ?
                            null :
                            auxStatus.TamperSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.TamperSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.TamperSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.TamperSensor)} {auxStatus.TamperSensor}")
                            },
                            InternalTamperSensor: propertyInfo.PropertyName != nameof(auxStatus.InternalTamperSensor) ?
                            null :
                            auxStatus.InternalTamperSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.InternalTamperSensor)} {auxStatus.InternalTamperSensor}")
                            },
                            SeismicSensor: propertyInfo.PropertyName != nameof(auxStatus.SeismicSensor) ?
                            null :
                            auxStatus.SeismicSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.SeismicSensor)} {auxStatus.SeismicSensor}")
                            },
                            HeatSensor: propertyInfo.PropertyName != nameof(auxStatus.HeatSensor) ?
                            null :
                            auxStatus.HeatSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.HeatSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.HeatSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.HeatSensor)} {auxStatus.HeatSensor}")
                            },
                            ProximitySensor: propertyInfo.PropertyName != nameof(auxStatus.ProximitySensor) ?
                            null :
                            auxStatus.ProximitySensor switch
                            {
                                AuxiliariesStatusClass.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.Present,
                                AuxiliariesStatusClass.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.NotPresent,
                                AuxiliariesStatusClass.PresenceSensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.ProximitySensor)} {auxStatus.ProximitySensor}")
                            },
                            AmbientLightSensor: propertyInfo.PropertyName != nameof(auxStatus.AmbientLightSensor) ?
                            null :
                            auxStatus.AmbientLightSensor switch
                            {
                                AuxiliariesStatusClass.AmbientLightSensorEnum.Dark => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.Dark,
                                AuxiliariesStatusClass.AmbientLightSensorEnum.Light => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.Light,
                                AuxiliariesStatusClass.AmbientLightSensorEnum.MediumLight => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.MediumLight,
                                AuxiliariesStatusClass.AmbientLightSensorEnum.VeryLight => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.VeryLight,
                                AuxiliariesStatusClass.AmbientLightSensorEnum.VeryDark => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.VeryDark,
                                AuxiliariesStatusClass.AmbientLightSensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.AmbientLightSensor)} {auxStatus.AmbientLightSensor}")
                            },
                            EnhancedAudioSensor: propertyInfo.PropertyName != nameof(auxStatus.EnhancedAudioSensor) ?
                            null :
                            auxStatus.EnhancedAudioSensor switch
                            {
                                AuxiliariesStatusClass.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.Present,
                                AuxiliariesStatusClass.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.NotPresent,
                                AuxiliariesStatusClass.PresenceSensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.EnhancedAudioSensor)} {auxStatus.EnhancedAudioSensor}")
                            },
                            BootSwitchSensor: propertyInfo.PropertyName != nameof(auxStatus.BootSwitchSensor) ?
                            null :
                            auxStatus.BootSwitchSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.BootSwitchSensor)} {auxStatus.BootSwitchSensor}")
                            },
                            ConsumerDisplaySensor: propertyInfo.PropertyName != nameof(auxStatus.DisplaySensor) ?
                            null :
                            auxStatus.DisplaySensor switch
                            {
                                AuxiliariesStatusClass.DisplaySensorEnum.On => XFS4IoT.Auxiliaries.ConsumerDisplaySensorStateEnum.On,
                                AuxiliariesStatusClass.DisplaySensorEnum.Off => XFS4IoT.Auxiliaries.ConsumerDisplaySensorStateEnum.Off,
                                AuxiliariesStatusClass.DisplaySensorEnum.DisplayError => XFS4IoT.Auxiliaries.ConsumerDisplaySensorStateEnum.DisplayError,
                                AuxiliariesStatusClass.DisplaySensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.DisplaySensor)} {auxStatus.DisplaySensor}")
                            },
                            OperatorCallButtonSensor: propertyInfo.PropertyName != nameof(auxStatus.OperatorCallButtonSensor) ?
                            null :
                            auxStatus.OperatorCallButtonSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.OperatorCallButtonSensor)} {auxStatus.OperatorCallButtonSensor}")
                            },
                            HandsetSensor: propertyInfo.PropertyName != nameof(auxStatus.HandsetSensor) ?
                            null :
                            auxStatus.HandsetSensor switch
                            {
                                AuxiliariesStatusClass.HandsetSensorStatusEnum.OnTheHook => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.OnTheHook,
                                AuxiliariesStatusClass.HandsetSensorStatusEnum.OffTheHook => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.OffTheHook,
                                AuxiliariesStatusClass.HandsetSensorStatusEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.HandsetSensor)} {auxStatus.HandsetSensor}")
                            },
                            HeadsetMicrophoneSensor: propertyInfo.PropertyName != nameof(auxStatus.HeadsetMicrophoneSensor) ?
                            null :
                            auxStatus.HeadsetMicrophoneSensor switch
                            {
                                AuxiliariesStatusClass.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.Present,
                                AuxiliariesStatusClass.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.NotPresent,
                                AuxiliariesStatusClass.PresenceSensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.HeadsetMicrophoneSensor)} {auxStatus.HeadsetMicrophoneSensor}")
                            },
                            FasciaMicrophoneSensor: propertyInfo.PropertyName != nameof(auxStatus.FasciaMicrophoneSensor) ?
                            null :
                            auxStatus.FasciaMicrophoneSensor switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.FasciaMicrophoneSensor)} {auxStatus.FasciaMicrophoneSensor}")
                            },
                            VandalShield: propertyInfo.PropertyName != nameof(auxStatus.VandalShield) ?
                            null :
                            auxStatus.VandalShield switch
                            {
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Open => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Open,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Closed => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Closed,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Locked => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Locked,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Tampered => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Tampered,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.PartiallyOpen => XFS4IoT.Auxiliaries.VandalShieldStateEnum.PartiallyOpen,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Service => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Service,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Jammed => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Jammed,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.Keyboard => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Keyboard,
                                AuxiliariesStatusClass.VandalShieldStatusEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.OperatorSwitch)} {auxStatus.OperatorSwitch}")
                            },
                            OpenClosedIndicator: propertyInfo.PropertyName != nameof(auxStatus.OpenClosedIndicator) ?
                            null :
                            auxStatus.OpenClosedIndicator switch
                            {
                                AuxiliariesStatusClass.OpenClosedIndicatorEnum.Open => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.Open,
                                AuxiliariesStatusClass.OpenClosedIndicatorEnum.Closed => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.Closed,
                                AuxiliariesStatusClass.OpenClosedIndicatorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.OpenClosedIndicator)} {auxStatus.OpenClosedIndicator}")
                            },
                            Audio: propertyInfo.PropertyName != nameof(auxStatus.AudioRate) &&
                                   propertyInfo.PropertyName != nameof(auxStatus.AudioSignal) ?
                            null :
                            new(
                                Rate: propertyInfo.PropertyName != nameof(auxStatus.AudioRate) ?
                                null :
                                auxStatus.AudioRate switch
                                {
                                    AuxiliariesStatusClass.AudioRateEnum.On => XFS4IoT.Auxiliaries.AudioStateClass.RateEnum.On,
                                    AuxiliariesStatusClass.AudioRateEnum.Off => XFS4IoT.Auxiliaries.AudioStateClass.RateEnum.Off,
                                    AuxiliariesStatusClass.AudioRateEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected audio rate status is specified. {auxStatus.AudioRate}")
                                },
                                Signal: propertyInfo.PropertyName != nameof(auxStatus.AudioSignal) ?
                                null :
                                auxStatus.AudioSignal switch
                                {
                                    AuxiliariesStatusClass.AudioSignalEnum.Warning => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Warning,
                                    AuxiliariesStatusClass.AudioSignalEnum.Exclamation => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Exclamation,
                                    AuxiliariesStatusClass.AudioSignalEnum.Error => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Error,
                                    AuxiliariesStatusClass.AudioSignalEnum.Critical => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Critical,
                                    AuxiliariesStatusClass.AudioSignalEnum.Keypress => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Keypress,
                                    AuxiliariesStatusClass.AudioSignalEnum.NotAvailable => null,
                                    _ => throw new InternalErrorException($"Unexpected audio signal is specified. {auxStatus.AudioSignal}")
                                }
                                ),
                            Heating: propertyInfo.PropertyName != nameof(auxStatus.Heating) ?
                            null :
                            auxStatus.Heating switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.HeatingStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.HeatingStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.Heating)} {auxStatus.Heating}")
                            },
                            ConsumerDisplayBacklight: propertyInfo.PropertyName != nameof(auxStatus.ConsumerDisplayBacklight) ?
                            null :
                            auxStatus.ConsumerDisplayBacklight switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.ConsumerDisplayBacklight)} {auxStatus.ConsumerDisplayBacklight}")
                            },
                            SignageDisplay: propertyInfo.PropertyName != nameof(auxStatus.SignageDisplay) ?
                            null :
                            auxStatus.SignageDisplay switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.SignageDisplay)} {auxStatus.SignageDisplay}")
                            },
                            Volume: propertyInfo.PropertyName != nameof(auxStatus.Volume) ?
                            null :
                            auxStatus.Volume == 0 ? null : auxStatus.Volume,
                            UPS: propertyInfo.PropertyName != nameof(auxStatus.UPS) ?
                            null :
                            new(
                                Low: !auxStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Good) && auxStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Low),
                                Engaged: auxStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Engaged),
                                Powering: auxStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Powering),
                                Recovered: auxStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Recovered)
                                ),
                            AudibleAlarm: propertyInfo.PropertyName != nameof(auxStatus.AudibleAlarm) ?
                            null :
                            auxStatus.AudibleAlarm switch
                            {
                                AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.On,
                                AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.Off,
                                AuxiliariesStatusClass.SensorEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.AudibleAlarm)} {auxStatus.AudibleAlarm}")
                            },
                            EnhancedAudioControl: propertyInfo.PropertyName != nameof(auxStatus.EnhancedAudioControl) ?
                            null :
                            auxStatus.EnhancedAudioControl switch
                            {
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioSemiAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioManual => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioManual,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioSemiAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioManual => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioManual,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.EnhancedAudioControl)} {auxStatus.EnhancedAudioControl}")
                            },
                            EnhancedMicrophoneControl: propertyInfo.PropertyName != nameof(auxStatus.EnhancedMicrophoneControl) ?
                            null :
                            auxStatus.EnhancedMicrophoneControl switch
                            {
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioSemiAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioManual => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioManual,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioSemiAuto,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioManual => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioManual,
                                AuxiliariesStatusClass.EnhancedAudioControlEnum.NotAvailable => null,
                                _ => throw new InternalErrorException($"Unexpected auxiliaries status is specified. {nameof(auxStatus.EnhancedMicrophoneControl)} {auxStatus.EnhancedMicrophoneControl}")
                            },
                            MicrophoneVolume: propertyInfo.PropertyName != nameof(auxStatus.MicrophoneVolume) ?
                            null :
                            auxStatus.MicrophoneVolume == 0 ? null : auxStatus.MicrophoneVolume)));
                }
            }
            if (sender.GetType() == typeof(CashAcceptorStatusClass))
            {
                CashAcceptorStatusClass cashAcceptor = sender as CashAcceptorStatusClass;
                cashAcceptor.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    CashAcceptor: new(
                        IntermediateStacker: propertyInfo.PropertyName != nameof(cashAcceptor.IntermediateStacker) ?
                        null :
                        cashAcceptor.IntermediateStacker switch
                        {
                            CashAcceptorStatusClass.IntermediateStackerEnum.Full => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.Full,
                            CashAcceptorStatusClass.IntermediateStackerEnum.Empty => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.Empty,
                            CashAcceptorStatusClass.IntermediateStackerEnum.NotEmpty => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.NotEmpty,
                            CashAcceptorStatusClass.IntermediateStackerEnum.Unknown => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.Unknown,
                            CashAcceptorStatusClass.IntermediateStackerEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unsupported intermediate stacker status specified. {cashAcceptor.IntermediateStacker}"),
                        },
                        StackerItems: propertyInfo.PropertyName != nameof(cashAcceptor.StackerItems) ?
                        null :
                        cashAcceptor.StackerItems switch
                        {
                            CashAcceptorStatusClass.StackerItemsEnum.AccessUnknown => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.AccessUnknown,
                            CashAcceptorStatusClass.StackerItemsEnum.NoCustomerAccess => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.NoCustomerAccess,
                            CashAcceptorStatusClass.StackerItemsEnum.CustomerAccess => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.CustomerAccess,
                            CashAcceptorStatusClass.StackerItemsEnum.NoItems => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.NoItems,
                            CashAcceptorStatusClass.StackerItemsEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unsupported stacker item status specified. {cashAcceptor.StackerItems}"),
                        },
                        BanknoteReader: propertyInfo.PropertyName != nameof(cashAcceptor.BanknoteReader) ?
                        null :
                        cashAcceptor.BanknoteReader switch
                        {
                            CashAcceptorStatusClass.BanknoteReaderEnum.Ok => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.Ok,
                            CashAcceptorStatusClass.BanknoteReaderEnum.Inoperable => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.Inoperable,
                            CashAcceptorStatusClass.BanknoteReaderEnum.Unknown => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.Unknown,
                            CashAcceptorStatusClass.BanknoteReaderEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unsupported intermediate stacker status specified. {cashAcceptor.IntermediateStacker}"),
                        },
                        DropBox: propertyInfo.PropertyName != nameof(cashAcceptor.DropBox) ?
                        null : cashAcceptor.DropBox)
                    ));
            }
            if (sender.GetType() == typeof(CashDispenserStatusClass))
            {
                CashDispenserStatusClass cashDispenser = sender as CashDispenserStatusClass;
                cashDispenser.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    CashDispenser: new(
                        IntermediateStacker: propertyInfo.PropertyName != nameof(cashDispenser.IntermediateStacker) ?
                        null :
                        cashDispenser.IntermediateStacker switch
                        {
                            CashDispenserStatusClass.IntermediateStackerEnum.NotEmptyUnknown => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotEmptyUnknown,
                            CashDispenserStatusClass.IntermediateStackerEnum.NotEmptyCustomer => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotEmptyCustomer,
                            CashDispenserStatusClass.IntermediateStackerEnum.Empty => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.Empty,
                            CashDispenserStatusClass.IntermediateStackerEnum.NotEmpty => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotEmpty,
                            CashDispenserStatusClass.IntermediateStackerEnum.Unknown => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.Unknown,
                            CashDispenserStatusClass.IntermediateStackerEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unsupported intermediate stacker status specified. {cashDispenser.IntermediateStacker}"),
                        })
                    ));
            }

            if (sender.GetType() == typeof(CashManagementStatusClass.PositionStatusClass))
            {
                CashManagementStatusClass.PositionStatusClass positionStatus = sender as CashManagementStatusClass.PositionStatusClass;
                positionStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                if (positionStatus.CashDispenserPosition is not null)
                {
                    XFS4IoT.CashDispenser.OutPosClass.ShutterEnum? outShutterStatus = propertyInfo.PropertyName != nameof(positionStatus.Shutter) ?
                        null :
                        positionStatus.Shutter switch
                        {
                            CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Open,
                            CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Closed,
                            CashManagementStatusClass.ShutterEnum.JammedClosed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedClosed,
                            CashManagementStatusClass.ShutterEnum.JammedPartiallyOpen => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedPartiallyOpen,
                            CashManagementStatusClass.ShutterEnum.JammedUnknown => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedUnknown,
                            CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Unknown,
                            CashManagementStatusClass.ShutterEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unsupported shutter status specified. {positionStatus.Shutter}"),
                        };

                    XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum? outPositionStatus = propertyInfo.PropertyName != nameof(positionStatus.PositionStatus) ?
                    null :
                    positionStatus.PositionStatus switch
                    {
                        CashManagementStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Empty,
                        CashManagementStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.NotEmpty,
                        CashManagementStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Unknown,
                        CashManagementStatusClass.PositionStatusEnum.NotSupported => null,
                        _ => throw new InternalErrorException($"Unsupported position status specified. {positionStatus.PositionStatus}"),
                    };

                    XFS4IoT.CashDispenser.OutPosClass.TransportEnum? outTransport = propertyInfo.PropertyName != nameof(positionStatus.Transport) ?
                    null :
                    positionStatus.Transport switch
                    {
                        CashManagementStatusClass.TransportEnum.Ok => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Ok,
                        CashManagementStatusClass.TransportEnum.Inoperative => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Inoperative,
                        CashManagementStatusClass.TransportEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Unknown,
                        CashManagementStatusClass.TransportEnum.NotSupported => null,
                        _ => throw new InternalErrorException($"Unsupported transport specified. {positionStatus.Transport}"),
                    };

                    XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum? outTransportStatus = propertyInfo.PropertyName != nameof(positionStatus.TransportStatus) ?
                    null :
                    positionStatus.TransportStatus switch
                    {
                        CashManagementStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Empty,
                        CashManagementStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmpty,
                        CashManagementStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmptyCustomer,
                        CashManagementStatusClass.TransportStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Unknown,
                        CashManagementStatusClass.TransportStatusEnum.NotSupported => null,
                        _ => throw new InternalErrorException($"Unsupported transport status specified. {positionStatus.TransportStatus}"),
                    };

                    CashManagementCapabilitiesClass.OutputPositionEnum pos = (CashManagementCapabilitiesClass.OutputPositionEnum)positionStatus.CashDispenserPosition;
                    List<CashManagementCapabilitiesClass.OutputPositionEnum> positions = Enum.GetValues<CashManagementCapabilitiesClass.OutputPositionEnum>().Where(e => pos.HasFlag(e) && e != 0 /*NotSupported*/).ToList();
                    if (positions.Count == 0)
                    {
                        positions = null;
                    }

                    List<XFS4IoT.CashDispenser.OutPosClass> positionsStatus = [];
                    foreach (var position in positions)
                    {
                        positionsStatus.Add(new(
                            Position: position switch
                            {
                                CashManagementCapabilitiesClass.OutputPositionEnum.Bottom => XFS4IoT.CashManagement.OutputPositionEnum.OutBottom,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Right => XFS4IoT.CashManagement.OutputPositionEnum.OutRight,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Front => XFS4IoT.CashManagement.OutputPositionEnum.OutFront,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Rear => XFS4IoT.CashManagement.OutputPositionEnum.OutRear,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Left => XFS4IoT.CashManagement.OutputPositionEnum.OutLeft,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Top => XFS4IoT.CashManagement.OutputPositionEnum.OutTop,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Center => XFS4IoT.CashManagement.OutputPositionEnum.OutCenter,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Default => XFS4IoT.CashManagement.OutputPositionEnum.OutDefault,
                                _ => throw new InternalErrorException($"Unsupported cash management output position specified. {position}"),

                            },
                            Shutter: outShutterStatus,
                            PositionStatus: outPositionStatus,
                            Transport: outTransport,
                            TransportStatus: outTransportStatus)
                            );
                    }

                    await StatusChangedEvent(new(
                        CashDispenser: new(
                            Positions: positionsStatus)
                        ));
                }
                if (positionStatus.CashAcceptorPosition is not null)
                {
                    CashManagementCapabilitiesClass.PositionEnum pos = (CashManagementCapabilitiesClass.PositionEnum)positionStatus.CashAcceptorPosition;

                    XFS4IoT.CashAcceptor.PositionClass.ShutterEnum? outShutterStatus = propertyInfo.PropertyName != nameof(positionStatus.Shutter) ?
                        null :
                        positionStatus.Shutter switch
                        {
                            CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.Open,
                            CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.Closed,
                            CashManagementStatusClass.ShutterEnum.JammedClosed => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedClosed,
                            CashManagementStatusClass.ShutterEnum.JammedPartiallyOpen => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedPartiallyOpen,
                            CashManagementStatusClass.ShutterEnum.JammedUnknown => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedUnknown,
                            CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.Unknown,
                            CashManagementStatusClass.ShutterEnum.NotSupported => null,
                            _ => throw new InternalErrorException($"Unsupported shutter status specified. {positionStatus.Shutter}"),
                        };

                    XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum? outPositionStatus = propertyInfo.PropertyName != nameof(positionStatus.PositionStatus) ?
                    null :
                    positionStatus.PositionStatus switch
                    {
                        CashManagementStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.Empty,
                        CashManagementStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.NotEmpty,
                        CashManagementStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.Unknown,
                        CashManagementStatusClass.PositionStatusEnum.NotSupported => null,
                        _ => throw new InternalErrorException($"Unsupported position status specified. {positionStatus.PositionStatus}"),
                    };

                    XFS4IoT.CashAcceptor.PositionClass.TransportEnum? outTransport = propertyInfo.PropertyName != nameof(positionStatus.Transport) ?
                    null :
                    positionStatus.Transport switch
                    {
                        CashManagementStatusClass.TransportEnum.Ok => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Ok,
                        CashManagementStatusClass.TransportEnum.Inoperative => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Inoperative,
                        CashManagementStatusClass.TransportEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Unknown,
                        CashManagementStatusClass.TransportEnum.NotSupported => null,
                        _ => throw new InternalErrorException($"Unsupported transport specified. {positionStatus.Transport}"),
                    };

                    XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum? outTransportStatus = propertyInfo.PropertyName != nameof(positionStatus.TransportStatus) ?
                    null :
                    positionStatus.TransportStatus switch
                    {
                        CashManagementStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.Empty,
                        CashManagementStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotEmpty,
                        CashManagementStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotEmptyCustomer,
                        CashManagementStatusClass.TransportStatusEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.Unknown,
                        CashManagementStatusClass.TransportStatusEnum.NotSupported => null,
                        _ => throw new InternalErrorException($"Unsupported transport status specified. {positionStatus.TransportStatus}"),
                    };

                    List<CashManagementCapabilitiesClass.PositionEnum> positions = Enum.GetValues<CashManagementCapabilitiesClass.PositionEnum>().Where(e => pos.HasFlag(e) && e != 0 /*NotSupported*/).ToList();
                    if (positions.Count == 0)
                    {
                        positions = null;
                    }

                    List<XFS4IoT.CashAcceptor.PositionClass> positionsStatus = [];
                    foreach (var position in positions)
                    {
                        positionsStatus.Add(new(
                            Position: position switch
                            {
                                CashManagementCapabilitiesClass.PositionEnum.InBottom => XFS4IoT.CashManagement.PositionEnum.InBottom,
                                CashManagementCapabilitiesClass.PositionEnum.InRight => XFS4IoT.CashManagement.PositionEnum.InRight,
                                CashManagementCapabilitiesClass.PositionEnum.InFront => XFS4IoT.CashManagement.PositionEnum.InFront,
                                CashManagementCapabilitiesClass.PositionEnum.InRear => XFS4IoT.CashManagement.PositionEnum.InRear,
                                CashManagementCapabilitiesClass.PositionEnum.InLeft => XFS4IoT.CashManagement.PositionEnum.InLeft,
                                CashManagementCapabilitiesClass.PositionEnum.InCenter => XFS4IoT.CashManagement.PositionEnum.InCenter,
                                CashManagementCapabilitiesClass.PositionEnum.InTop => XFS4IoT.CashManagement.PositionEnum.InTop,
                                CashManagementCapabilitiesClass.PositionEnum.InDefault => XFS4IoT.CashManagement.PositionEnum.InDefault,
                                CashManagementCapabilitiesClass.PositionEnum.OutBottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                                CashManagementCapabilitiesClass.PositionEnum.OutRight => XFS4IoT.CashManagement.PositionEnum.OutRight,
                                CashManagementCapabilitiesClass.PositionEnum.OutFront => XFS4IoT.CashManagement.PositionEnum.OutFront,
                                CashManagementCapabilitiesClass.PositionEnum.OutRear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                                CashManagementCapabilitiesClass.PositionEnum.OutLeft => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                                CashManagementCapabilitiesClass.PositionEnum.OutTop => XFS4IoT.CashManagement.PositionEnum.OutTop,
                                CashManagementCapabilitiesClass.PositionEnum.OutCenter => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                                CashManagementCapabilitiesClass.PositionEnum.OutDefault => XFS4IoT.CashManagement.PositionEnum.InBottom,
                                _ => throw new InternalErrorException($"Unsupported cash management position specified. {positionStatus.CashAcceptorPosition}"),

                            },
                            Shutter: outShutterStatus,
                            PositionStatus: outPositionStatus,
                            Transport: outTransport,
                            TransportStatus: outTransportStatus)
                            );
                    }

                    await StatusChangedEvent(new(
                        CashAcceptor: new(
                            Positions: positionsStatus)
                        ));
                }
            }

            if (sender.GetType() == typeof(PrinterStatusClass) ||
                sender.GetType() == typeof(PrinterStatusClass.SupplyStatusClass))
            {
                if (sender.GetType() == typeof(PrinterStatusClass))
                {
                    PrinterStatusClass printerStatus = sender as PrinterStatusClass;
                    printerStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        Printer: new(
                            Media: propertyInfo.PropertyName != nameof(printerStatus.Media) ?
                            null :
                            printerStatus.Media switch
                            {
                                PrinterStatusClass.MediaEnum.Present => XFS4IoT.Printer.StatusClass.MediaEnum.Present,
                                PrinterStatusClass.MediaEnum.Entering => XFS4IoT.Printer.StatusClass.MediaEnum.Entering,
                                PrinterStatusClass.MediaEnum.Retracted => XFS4IoT.Printer.StatusClass.MediaEnum.Retracted,
                                PrinterStatusClass.MediaEnum.NotPresent => XFS4IoT.Printer.StatusClass.MediaEnum.NotPresent,
                                PrinterStatusClass.MediaEnum.Jammed => XFS4IoT.Printer.StatusClass.MediaEnum.Jammed,
                                PrinterStatusClass.MediaEnum.Unknown => XFS4IoT.Printer.StatusClass.MediaEnum.Unknown,
                                PrinterStatusClass.MediaEnum.NotSupported => null,
                                _ => throw new InternalErrorException($"Unsupported media status specified. {printerStatus.Media}"),
                            },
                            Toner: propertyInfo.PropertyName != nameof(printerStatus.Toner) ?
                            null :
                            printerStatus.Toner switch
                            {
                                PrinterStatusClass.TonerEnum.Full => XFS4IoT.Printer.StatusClass.TonerEnum.Full,
                                PrinterStatusClass.TonerEnum.Low => XFS4IoT.Printer.StatusClass.TonerEnum.Low,
                                PrinterStatusClass.TonerEnum.Out => XFS4IoT.Printer.StatusClass.TonerEnum.Out,
                                PrinterStatusClass.TonerEnum.Unknown => XFS4IoT.Printer.StatusClass.TonerEnum.Unknown,
                                PrinterStatusClass.TonerEnum.NotSupported => null,
                                _ => throw new InternalErrorException($"Unsupported toner status specified. {printerStatus.Toner}"),
                            },
                            Ink: propertyInfo.PropertyName != nameof(printerStatus.Ink) ?
                            null :
                            printerStatus.Ink switch
                            {
                                PrinterStatusClass.InkEnum.Full => XFS4IoT.Printer.StatusClass.InkEnum.Full,
                                PrinterStatusClass.InkEnum.Low => XFS4IoT.Printer.StatusClass.InkEnum.Low,
                                PrinterStatusClass.InkEnum.Out => XFS4IoT.Printer.StatusClass.InkEnum.Out,
                                PrinterStatusClass.InkEnum.Unknown => XFS4IoT.Printer.StatusClass.InkEnum.Unknown,
                                PrinterStatusClass.InkEnum.NotSupported => null,
                                _ => throw new InternalErrorException($"Unsupported ink status specified. {printerStatus.Ink}"),
                            },
                            Lamp: propertyInfo.PropertyName != nameof(printerStatus.Lamp) ?
                            null :
                            printerStatus.Lamp switch
                            {
                                PrinterStatusClass.LampEnum.Ok => XFS4IoT.Printer.StatusClass.LampEnum.Ok,
                                PrinterStatusClass.LampEnum.Fading => XFS4IoT.Printer.StatusClass.LampEnum.Fading,
                                PrinterStatusClass.LampEnum.Inop => XFS4IoT.Printer.StatusClass.LampEnum.Inop,
                                PrinterStatusClass.LampEnum.Unknown => XFS4IoT.Printer.StatusClass.LampEnum.Unknown,
                                PrinterStatusClass.LampEnum.NotSupported => null,
                                _ => throw new InternalErrorException($"Unsupported lamp status specified. {printerStatus.Lamp}"),
                            },
                            MediaOnStacker: propertyInfo.PropertyName != nameof(printerStatus.MediaOnStacker) ?
                            null :
                            printerStatus.MediaOnStacker < 0 ? null : printerStatus.MediaOnStacker,
                            BlackMarkMode: propertyInfo.PropertyName != nameof(printerStatus.BlackMarkMode) ?
                            null :
                            printerStatus.BlackMarkMode switch
                            {
                                PrinterStatusClass.BlackMarkModeEnum.On => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.On,
                                PrinterStatusClass.BlackMarkModeEnum.Off => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.Off,
                                PrinterStatusClass.BlackMarkModeEnum.Unknown => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.Unknown,
                                PrinterStatusClass.BlackMarkModeEnum.NotSupported => null,
                                _ => throw new InternalErrorException($"Unsupported black mark status specified. {printerStatus.BlackMarkMode}"),
                            })
                        ));
                }
                else
                {
                    PrinterStatusClass.SupplyStatusClass supplyStatus = sender as PrinterStatusClass.SupplyStatusClass;
                    supplyStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    XFS4IoT.Printer.PaperTypeEnum? typeStatus = propertyInfo.PropertyName != nameof(supplyStatus.PaperType) ?
                        null :
                        supplyStatus.PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Unknown => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                            _ => null,
                        };

                    XFS4IoT.Printer.PaperSupplyEnum? paperStatus = propertyInfo.PropertyName != nameof(supplyStatus.PaperSupply) ?
                        null :
                        supplyStatus.PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => null,
                        };

                    if (paperStatus is not null &&
                        supplyStatus.Source is not null)
                    {
                        XFS4IoT.Printer.StatusClass.PaperClass paper = new();

                        if (supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.Upper ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.Lower ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.External ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.AUX ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.AUX2 ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.Park)
                        {
                            paper = new(
                                Upper: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.Upper ?
                                null : paperStatus,
                                Lower: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.Lower ?
                                null : paperStatus,
                                External: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.External ?
                                null : paperStatus,
                                Aux: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.AUX ?
                                null : paperStatus,
                                Aux2: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.AUX2 ?
                                null : paperStatus,
                                Park: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.Park ?
                                null : paperStatus);
                        }
                        if (!string.IsNullOrEmpty(supplyStatus.CustomSource))
                        {
                            paper.ExtendedProperties.Add(supplyStatus.CustomSource, (XFS4IoT.Printer.PaperSupplyEnum)paperStatus);
                        }

                        await StatusChangedEvent(new(
                            Printer: new(Paper: paper)
                        ));
                    }

                    if (typeStatus is not null &&
                        supplyStatus.Source is not null)
                    {
                        XFS4IoT.Printer.StatusClass.PaperTypeClass type = new();

                        if (supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.Upper ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.Lower ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.External ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.AUX ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.AUX2 ||
                            supplyStatus.Source == PrinterStatusClass.PaperSourceEnum.Park)
                        {
                            type = new(
                                Upper: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.Upper ?
                                null : typeStatus,
                                Lower: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.Lower ?
                                null : typeStatus,
                                External: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.External ?
                                null : typeStatus,
                                Aux: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.AUX ?
                                null : typeStatus,
                                Aux2: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.AUX2 ?
                                null : typeStatus,
                                Park: supplyStatus.Source != PrinterStatusClass.PaperSourceEnum.Park ?
                                null : typeStatus);
                        }
                        if (!string.IsNullOrEmpty(supplyStatus.CustomSource))
                        {
                            type.ExtendedProperties.Add(supplyStatus.CustomSource, (XFS4IoT.Printer.PaperTypeEnum)typeStatus);
                        }

                        await StatusChangedEvent(new(
                            Printer: new(PaperType: type)
                        ));
                    }
                }
            }
            if (sender.GetType() == typeof(CheckScannerStatusClass) ||
                sender.GetType() == typeof(CheckScannerStatusClass.PositionStatusClass))
            {
                if (sender.GetType() == typeof(CheckScannerStatusClass))
                {
                    CheckScannerStatusClass checkScannerStatus = sender as CheckScannerStatusClass;
                    checkScannerStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        Check: new(
                            Acceptor: propertyInfo.PropertyName != nameof(checkScannerStatus.Acceptor) ?
                            null :
                            checkScannerStatus.Acceptor switch
                            {
                                CheckScannerStatusClass.AcceptorEnum.Ok => XFS4IoT.Check.StatusClass.AcceptorEnum.Ok,
                                CheckScannerStatusClass.AcceptorEnum.Attention => XFS4IoT.Check.StatusClass.AcceptorEnum.State,
                                CheckScannerStatusClass.AcceptorEnum.Stop => XFS4IoT.Check.StatusClass.AcceptorEnum.Stop,
                                CheckScannerStatusClass.AcceptorEnum.Unknown => XFS4IoT.Check.StatusClass.AcceptorEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected acceptor status specified. {checkScannerStatus.Acceptor}")
                            },
                            Media: propertyInfo.PropertyName != nameof(checkScannerStatus.Media) ?
                            null :
                            checkScannerStatus.Media switch
                            {
                                CheckScannerStatusClass.MediaEnum.Present => XFS4IoT.Check.StatusClass.MediaEnum.Present,
                                CheckScannerStatusClass.MediaEnum.NotPresent => XFS4IoT.Check.StatusClass.MediaEnum.NotPresent,
                                CheckScannerStatusClass.MediaEnum.Position => XFS4IoT.Check.StatusClass.MediaEnum.Position,
                                CheckScannerStatusClass.MediaEnum.Jammed => XFS4IoT.Check.StatusClass.MediaEnum.Jammed,
                                CheckScannerStatusClass.MediaEnum.Unknown => XFS4IoT.Check.StatusClass.MediaEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected media status specified. {checkScannerStatus.Media}")
                            },
                            Toner: propertyInfo.PropertyName != nameof(checkScannerStatus.Toner) ?
                            null :
                            checkScannerStatus.Toner switch
                            {
                                CheckScannerStatusClass.TonerEnum.Out => XFS4IoT.Check.TonerEnum.Out,
                                CheckScannerStatusClass.TonerEnum.Full => XFS4IoT.Check.TonerEnum.Full,
                                CheckScannerStatusClass.TonerEnum.Low => XFS4IoT.Check.TonerEnum.Low,
                                CheckScannerStatusClass.TonerEnum.Unknown => XFS4IoT.Check.TonerEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected acceptor status specified. {checkScannerStatus.Toner}")
                            },
                            Ink: propertyInfo.PropertyName != nameof(checkScannerStatus.Ink) ?
                            null :
                            checkScannerStatus.Ink switch
                            {
                                CheckScannerStatusClass.InkEnum.Out => XFS4IoT.Check.InkEnum.Out,
                                CheckScannerStatusClass.InkEnum.Full => XFS4IoT.Check.InkEnum.Full,
                                CheckScannerStatusClass.InkEnum.Low => XFS4IoT.Check.InkEnum.Low,
                                CheckScannerStatusClass.InkEnum.Unknown => XFS4IoT.Check.InkEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected ink status specified. {checkScannerStatus.Ink}")
                            },
                            FrontImageScanner: propertyInfo.PropertyName != nameof(checkScannerStatus.FrontImageScanner) ?
                            null :
                            checkScannerStatus.FrontImageScanner switch
                            {
                                CheckScannerStatusClass.ImageScannerEnum.Ok => XFS4IoT.Check.FrontImageScannerEnum.Ok,
                                CheckScannerStatusClass.ImageScannerEnum.Fading => XFS4IoT.Check.FrontImageScannerEnum.Fading,
                                CheckScannerStatusClass.ImageScannerEnum.Inoperative => XFS4IoT.Check.FrontImageScannerEnum.Inoperative,
                                CheckScannerStatusClass.ImageScannerEnum.Unknown => XFS4IoT.Check.FrontImageScannerEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected front image scanner specified. {checkScannerStatus.FrontImageScanner}")
                            },
                            BackImageScanner: propertyInfo.PropertyName != nameof(checkScannerStatus.BackImageScanner) ?
                            null :
                            checkScannerStatus.BackImageScanner switch
                            {
                                CheckScannerStatusClass.ImageScannerEnum.Ok => XFS4IoT.Check.BackImageScannerEnum.Ok,
                                CheckScannerStatusClass.ImageScannerEnum.Fading => XFS4IoT.Check.BackImageScannerEnum.Fading,
                                CheckScannerStatusClass.ImageScannerEnum.Inoperative => XFS4IoT.Check.BackImageScannerEnum.Inoperative,
                                CheckScannerStatusClass.ImageScannerEnum.Unknown => XFS4IoT.Check.BackImageScannerEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected back image scanner specified. {checkScannerStatus.BackImageScanner}")
                            },
                            MICRReader: propertyInfo.PropertyName != nameof(checkScannerStatus.MICRReader) ?
                            null :
                            checkScannerStatus.MICRReader switch
                            {
                                CheckScannerStatusClass.ImageScannerEnum.Ok => XFS4IoT.Check.MicrReaderEnum.Ok,
                                CheckScannerStatusClass.ImageScannerEnum.Fading => XFS4IoT.Check.MicrReaderEnum.Fading,
                                CheckScannerStatusClass.ImageScannerEnum.Inoperative => XFS4IoT.Check.MicrReaderEnum.Inoperative,
                                CheckScannerStatusClass.ImageScannerEnum.Unknown => XFS4IoT.Check.MicrReaderEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected MICR reader specified. {checkScannerStatus.MICRReader}")
                            },
                            Stacker: propertyInfo.PropertyName != nameof(checkScannerStatus.Stacker) ?
                            null :
                            checkScannerStatus.Stacker switch
                            {
                                CheckScannerStatusClass.StackerEnum.Empty => XFS4IoT.Check.StatusClass.StackerEnum.Empty,
                                CheckScannerStatusClass.StackerEnum.NotEmpty => XFS4IoT.Check.StatusClass.StackerEnum.NotEmpty,
                                CheckScannerStatusClass.StackerEnum.Full => XFS4IoT.Check.StatusClass.StackerEnum.Full,
                                CheckScannerStatusClass.StackerEnum.Inoperative => XFS4IoT.Check.StatusClass.StackerEnum.Inoperative,
                                CheckScannerStatusClass.StackerEnum.Unknown => XFS4IoT.Check.StatusClass.StackerEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected stacker status specified. {checkScannerStatus.Stacker}")
                            },
                            Rebuncher: propertyInfo.PropertyName != nameof(checkScannerStatus.ReBuncher) ?
                            null :
                            checkScannerStatus.ReBuncher switch
                            {
                                CheckScannerStatusClass.ReBuncherEnum.Empty => XFS4IoT.Check.StatusClass.RebuncherEnum.Empty,
                                CheckScannerStatusClass.ReBuncherEnum.NotEmpty => XFS4IoT.Check.StatusClass.RebuncherEnum.NotEmpty,
                                CheckScannerStatusClass.ReBuncherEnum.Full => XFS4IoT.Check.StatusClass.RebuncherEnum.Full,
                                CheckScannerStatusClass.ReBuncherEnum.Inoperative => XFS4IoT.Check.StatusClass.RebuncherEnum.Inoperative,
                                CheckScannerStatusClass.ReBuncherEnum.Unknown => XFS4IoT.Check.StatusClass.RebuncherEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected rebuncher status specified. {checkScannerStatus.ReBuncher}")
                            },
                            MediaFeeder: propertyInfo.PropertyName != nameof(checkScannerStatus.MediaFeeder) ?
                            null :
                            checkScannerStatus.MediaFeeder switch
                            {
                                CheckScannerStatusClass.MediaFeederEnum.Empty => XFS4IoT.Check.MediaFeederEnum.Empty,
                                CheckScannerStatusClass.MediaFeederEnum.NotEmpty => XFS4IoT.Check.MediaFeederEnum.NotEmpty,
                                CheckScannerStatusClass.MediaFeederEnum.Inoperative => XFS4IoT.Check.MediaFeederEnum.Inoperative,
                                CheckScannerStatusClass.MediaFeederEnum.Unknown => XFS4IoT.Check.MediaFeederEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected media feeder status specified. {checkScannerStatus.MediaFeeder}")
                            })
                        ));
                }
                else
                {
                    CheckScannerStatusClass.PositionStatusClass positionStatus = sender as CheckScannerStatusClass.PositionStatusClass;
                    positionStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    XFS4IoT.Check.PositionStatusClass input = null;
                    XFS4IoT.Check.PositionStatusClass output = null;
                    XFS4IoT.Check.PositionStatusClass refused = null;

                    XFS4IoT.Check.PositionStatusClass stat = new(
                        Shutter: propertyInfo.PropertyName != nameof(positionStatus.Shutter) ?
                        null :
                        positionStatus.Shutter switch
                        {
                            CheckScannerStatusClass.ShutterEnum.Open => XFS4IoT.Check.ShutterStateEnum.Open,
                            CheckScannerStatusClass.ShutterEnum.Closed => XFS4IoT.Check.ShutterStateEnum.Closed,
                            CheckScannerStatusClass.ShutterEnum.Jammed => XFS4IoT.Check.ShutterStateEnum.Jammed,
                            CheckScannerStatusClass.ShutterEnum.Unknown => XFS4IoT.Check.ShutterStateEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected shutter status specified. {positionStatus.Shutter}")
                        },
                        PositionStatus: propertyInfo.PropertyName != nameof(positionStatus.PositionStatus) ?
                        null :
                        positionStatus.PositionStatus switch
                        {
                            CheckScannerStatusClass.PositionStatusEnum.Empty => XFS4IoT.Check.PositionStatusClass.PositionStatusEnum.Empty,
                            CheckScannerStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.Check.PositionStatusClass.PositionStatusEnum.NotEmpty,
                            CheckScannerStatusClass.PositionStatusEnum.Unknown => XFS4IoT.Check.PositionStatusClass.PositionStatusEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected position status specified. {positionStatus.PositionStatus}")
                        },
                        Transport: propertyInfo.PropertyName != nameof(positionStatus.Transport) ?
                        null :
                        positionStatus.Transport switch
                        {
                            CheckScannerStatusClass.TransportEnum.Ok => XFS4IoT.Check.PositionStatusClass.TransportEnum.Ok,
                            CheckScannerStatusClass.TransportEnum.Inoperative => XFS4IoT.Check.PositionStatusClass.TransportEnum.Inoperative,
                            CheckScannerStatusClass.TransportEnum.Unknown => XFS4IoT.Check.PositionStatusClass.TransportEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected transport status specified. {positionStatus.Transport}")
                        },
                        TransportMediaStatus: propertyInfo.PropertyName != nameof(positionStatus.TransportMediaStatus) ?
                        null :
                        positionStatus.TransportMediaStatus switch
                        {
                            CheckScannerStatusClass.TransportMediaStatusEnum.Empty => XFS4IoT.Check.PositionStatusClass.TransportMediaStatusEnum.Empty,
                            CheckScannerStatusClass.TransportMediaStatusEnum.NotEmpty => XFS4IoT.Check.PositionStatusClass.TransportMediaStatusEnum.NotEmpty,
                            CheckScannerStatusClass.TransportMediaStatusEnum.Unknown => XFS4IoT.Check.PositionStatusClass.TransportMediaStatusEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected transport media status specified. {positionStatus.TransportMediaStatus}")
                        },
                        JammedShutterPosition: propertyInfo.PropertyName != nameof(positionStatus.JammedShutterPosition) ?
                        null :
                        positionStatus.JammedShutterPosition switch
                        {
                            CheckScannerStatusClass.JammedShutterPositionEnum.Open => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.Open,
                            CheckScannerStatusClass.JammedShutterPositionEnum.Closed => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.Closed,
                            CheckScannerStatusClass.JammedShutterPositionEnum.PartiallyOpen => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.PartiallyOpen,
                            CheckScannerStatusClass.JammedShutterPositionEnum.NotJammed => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.NotJammed,
                            CheckScannerStatusClass.JammedShutterPositionEnum.Unknown => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected jammed shutter position status specified. {positionStatus.JammedShutterPosition}")
                        });

                    positionStatus.Position.IsNotNull($"Unexpected Position property set to null. {nameof(positionStatus.Position)}");

                    CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum thisPosition = (CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum)positionStatus.Position;
                    if (thisPosition.HasFlag(CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum.Input))
                    {
                        input = stat;
                    }
                    if (thisPosition.HasFlag(CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum.Output))
                    {
                        output = stat;
                    }
                    if (thisPosition.HasFlag(CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum.Refused))
                    {
                        refused = stat;
                    }

                    await StatusChangedEvent(new(
                        Check: new(
                            Positions: new(
                                Input: input,
                                Output: output,
                                Refused: refused)
                            )
                        ));
                }
                if (sender.GetType() == typeof(MixedMediaStatusClass))
                {
                    MixedMediaStatusClass mixedMediaStatus = sender as MixedMediaStatusClass;
                    mixedMediaStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        MixedMedia: new(
                            Modes: new(
                                CashAccept: mixedMediaStatus.CurrentModes.HasFlag(MixedMedia.ModeTypeEnum.Cash),
                                CheckAccept: mixedMediaStatus.CurrentModes.HasFlag(MixedMedia.ModeTypeEnum.Check)
                                )
                            )
                        ));
                }
            }
            if (sender.GetType() == typeof(IBNSStatusClass) ||
                sender.GetType() == typeof(IBNSStatusClass.StateClass) ||
                sender.GetType() == typeof(IBNSStatusClass.WarningStateClass) ||
                sender.GetType() == typeof(IBNSStatusClass.ErrorStateClass) ||
                sender.GetType() == typeof(IBNSStatusClass.CustomInputStatusClass))
            {
                if (sender.GetType() == typeof(IBNSStatusClass))
                {
                    IBNSStatusClass ibnsStatus = sender as IBNSStatusClass;
                    ibnsStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        BanknoteNeutralization: new(
                            SafeDoor: propertyInfo.PropertyName != nameof(ibnsStatus.SafeDoorState) ?
                            null :
                            ibnsStatus.SafeDoorState switch
                            {
                                IBNSStatusClass.SafeDoorStateEnum.Fault => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.Fault,
                                IBNSStatusClass.SafeDoorStateEnum.DoorClosed => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.DoorClosed,
                                IBNSStatusClass.SafeDoorStateEnum.DoorOpened => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.DoorOpened,
                                IBNSStatusClass.SafeDoorStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.Disabled,
                                _ => throw new InternalErrorException($"Unexpected IBNS safe door status specified. {ibnsStatus.SafeDoorState}"),
                            },
                            SafeBolt: propertyInfo.PropertyName != nameof(ibnsStatus.SafeBoltState) ?
                            null :
                            ibnsStatus.SafeBoltState switch
                            {
                                IBNSStatusClass.SafeBoltStateEnum.Fault => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.Fault,
                                IBNSStatusClass.SafeBoltStateEnum.BoltLocked => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.BoltLocked,
                                IBNSStatusClass.SafeBoltStateEnum.BoltUnlocked => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.BoltUnlocked,
                                IBNSStatusClass.SafeBoltStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.Disabled,
                                _ => throw new InternalErrorException($"Unexpected IBNS safe bolt status specified. {ibnsStatus.SafeBoltState}"),
                            },
                            Tilt: propertyInfo.PropertyName != nameof(ibnsStatus.TiltState) ?
                            null :
                            ibnsStatus.TiltState switch
                            {
                                IBNSStatusClass.TiltStateEnum.Fault => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Fault,
                                IBNSStatusClass.TiltStateEnum.Tilted => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Tilted,
                                IBNSStatusClass.TiltStateEnum.NotTilted => XFS4IoT.BanknoteNeutralization.TiltStateEnum.NotTilted,
                                IBNSStatusClass.TiltStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Disabled,
                                _ => throw new InternalErrorException($"Unexpected IBNS tilt status specified. {ibnsStatus.TiltState}"),
                            },
                            Light: propertyInfo.PropertyName != nameof(ibnsStatus.LightState) ?
                            null :
                            ibnsStatus.LightState switch
                            {
                                IBNSStatusClass.LightStateEnum.Fault => XFS4IoT.BanknoteNeutralization.LightStateEnum.Fault,
                                IBNSStatusClass.LightStateEnum.NotConfigured => XFS4IoT.BanknoteNeutralization.LightStateEnum.NotConfigured,
                                IBNSStatusClass.LightStateEnum.Detected => XFS4IoT.BanknoteNeutralization.LightStateEnum.Detected,
                                IBNSStatusClass.LightStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.LightStateEnum.Disabled,
                                _ => throw new InternalErrorException($"Unexpected IBNS light status specified. {ibnsStatus.LightState}"),
                            },
                            Gas: propertyInfo.PropertyName != nameof(ibnsStatus.GasState) ?
                            null :
                            ibnsStatus.GasState switch
                            {
                                IBNSStatusClass.GasStateEnum.Fault => XFS4IoT.BanknoteNeutralization.GasStateEnum.Fault,
                                IBNSStatusClass.GasStateEnum.Initializing => XFS4IoT.BanknoteNeutralization.GasStateEnum.Initializing,
                                IBNSStatusClass.GasStateEnum.NotConfigured => XFS4IoT.BanknoteNeutralization.GasStateEnum.NotConfigured,
                                IBNSStatusClass.GasStateEnum.PartialWarningLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.PartialWarningLevel,
                                IBNSStatusClass.GasStateEnum.PartialCriticalLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.PartialCriticalLevel,
                                IBNSStatusClass.GasStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.GasStateEnum.Disabled,
                                IBNSStatusClass.GasStateEnum.WarningLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.WarningLevel,
                                IBNSStatusClass.GasStateEnum.CriticalLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.CriticalLevel,
                                IBNSStatusClass.GasStateEnum.NotDetected => XFS4IoT.BanknoteNeutralization.GasStateEnum.NotDetected,
                                _ => throw new InternalErrorException($"Unexpected IBNS GAS status specified. {ibnsStatus.GasState}"),
                            },
                            Temperature: propertyInfo.PropertyName != nameof(ibnsStatus.TemperatureState) ?
                            null :
                            ibnsStatus.TemperatureState switch
                            {
                                IBNSStatusClass.TemperatureStateEnum.Fault => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Fault,
                                IBNSStatusClass.TemperatureStateEnum.Healthy => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Ok,
                                IBNSStatusClass.TemperatureStateEnum.TooCold => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.TooCold,
                                IBNSStatusClass.TemperatureStateEnum.TooHot => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.TooHot,
                                IBNSStatusClass.TemperatureStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Disabled,
                                _ => throw new InternalErrorException($"Unexpected IBNS temperature status specified. {ibnsStatus.TemperatureState}"),
                            },
                            Seismic: propertyInfo.PropertyName != nameof(ibnsStatus.SeismicState) ?
                            null :
                            ibnsStatus.SeismicState switch
                            {
                                IBNSStatusClass.SeismicStateEnum.Fault => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.Fault,
                                IBNSStatusClass.SeismicStateEnum.NotConfigured => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.NotConfigured,
                                IBNSStatusClass.SeismicStateEnum.CriticalLevel => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.CriticalLevel,
                                IBNSStatusClass.SeismicStateEnum.WarningLevel => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.WarningLevel,
                                IBNSStatusClass.SeismicStateEnum.NotDetected => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.NotDetected,
                                IBNSStatusClass.SeismicStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.Disabled,
                                _ => throw new InternalErrorException($"Unexpected IBNS seismic status specified. {ibnsStatus.SeismicState}"),
                            })
                        ));
                }
                else if (sender.GetType() == typeof(IBNSStatusClass.StateClass))
                {
                    IBNSStatusClass.StateClass state = sender as IBNSStatusClass.StateClass;
                    state.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        BanknoteNeutralization: new(
                            State: new XFS4IoT.BanknoteNeutralization.StateClass(
                                Mode: propertyInfo.PropertyName != nameof(state.Mode) ?
                                null :
                                state.Mode switch
                                {
                                    IBNSStatusClass.StateClass.ModeEnum.NeutralizationTriggered => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.NeutralizationTriggered,
                                    IBNSStatusClass.StateClass.ModeEnum.Fault => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.Fault,
                                    IBNSStatusClass.StateClass.ModeEnum.Disarmed => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.Disarmed,
                                    IBNSStatusClass.StateClass.ModeEnum.Armed => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.Armed,
                                    _ => throw new InternalErrorException($"Unexpected mode state specified. {state.Mode}")
                                },
                                Submode: propertyInfo.PropertyName != nameof(state.SubMode) ?
                                null :
                                state.SubMode switch
                                {
                                    IBNSStatusClass.StateClass.SubModeEnum.AllSafeSensorsIgnored => XFS4IoT.BanknoteNeutralization.StateClass.SubmodeEnum.AllSafeSensorsIgnored,
                                    IBNSStatusClass.StateClass.SubModeEnum.ArmPending => XFS4IoT.BanknoteNeutralization.StateClass.SubmodeEnum.ArmPending,
                                    _ => throw new InternalErrorException($"Unexpected submode status specified. {state.SubMode}"),
                                })
                            ))
                        );
                }
                else if (sender.GetType() == typeof(IBNSStatusClass.WarningStateClass))
                {
                    IBNSStatusClass.WarningStateClass warningState = sender as IBNSStatusClass.WarningStateClass;
                    warningState.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        BanknoteNeutralization: new(
                            Warnings: new(
                                ProtectionArmingFault: propertyInfo.PropertyName != nameof(warningState.ProtectionArmingFault) ?
                                    null :
                                    warningState.ProtectionArmingFault,
                                ProtectionDisarmingFault: propertyInfo.PropertyName != nameof(warningState.ProtectionDisarmingFault) ?
                                    null :
                                    warningState.ProtectionDisarmingFault,
                                ExternalMainPowerOutage: propertyInfo.PropertyName != nameof(warningState.ExternalMainPowerOutage) ?
                                    null :
                                    warningState.ExternalMainPowerOutage,
                                StorageUnitLowPowerSupply: propertyInfo.PropertyName != nameof(warningState.StorageUnitLowPowerSupply) ?
                                    null :
                                    warningState.StorageUnitLowPowerSupply,
                                ArmedAutonomous: propertyInfo.PropertyName != nameof(warningState.ArmedAutonomous) ?
                                    null :
                                    warningState.ArmedAutonomous,
                                ArmedAlarm: propertyInfo.PropertyName != nameof(warningState.ArmedAlarm) ?
                                    null :
                                    warningState.ArmedAlarm,
                                GasWarningLevel: propertyInfo.PropertyName != nameof(warningState.GasWarningLevel) ?
                                    null :
                                    warningState.GasWarningLevel,
                                SeismicActivityWarningLevel: propertyInfo.PropertyName != nameof(warningState.SeismicActivityWarningLevel) ?
                                    null :
                                    warningState.SeismicActivityWarningLevel
                                ))
                        ));
                }
                else if (sender.GetType() == typeof(IBNSStatusClass.ErrorStateClass))
                {
                    IBNSStatusClass.ErrorStateClass errorState = sender as IBNSStatusClass.ErrorStateClass;
                    errorState.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    await StatusChangedEvent(new(
                        BanknoteNeutralization: new(
                            Errors: new(
                                ProtectionEnablingFailure: propertyInfo.PropertyName != nameof(errorState.ProtectionEnablingFailure) ?
                                    null :
                                    errorState.ProtectionEnablingFailure,
                                ProtectionDisarmingFailure: propertyInfo.PropertyName != nameof(errorState.ProtectionDisarmingFailure) ?
                                    null :
                                    errorState.ProtectionDisarmingFailure,
                                StorageUnitPowerSupplyFailure: propertyInfo.PropertyName != nameof(errorState.StorageUnitPowerSupplyFailure) ?
                                    null :
                                    errorState.StorageUnitPowerSupplyFailure,
                                BackupBatteryFailure: propertyInfo.PropertyName != nameof(errorState.BackupBatteryFailure) ?
                                    null :
                                    errorState.BackupBatteryFailure,
                                GasCriticalLevel: propertyInfo.PropertyName != nameof(errorState.GasCriticalLevel) ?
                                    null :
                                    errorState.GasCriticalLevel,
                                Light: propertyInfo.PropertyName != nameof(errorState.Light) ?
                                    null :
                                    errorState.Light,
                                Tilted: propertyInfo.PropertyName != nameof(errorState.Tilted) ?
                                    null :
                                    errorState.Tilted,
                                SeismicActivityCriticalLevel: propertyInfo.PropertyName != nameof(errorState.SeismicActivityCriticalLevel) ?
                                    null :
                                    errorState.SeismicActivityCriticalLevel)
                            ))
                        );
                }
                else if (sender.GetType() == typeof(IBNSStatusClass.CustomInputStatusClass))
                {
                    IBNSStatusClass.CustomInputStatusClass inputStatus = sender as IBNSStatusClass.CustomInputStatusClass;
                    inputStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    Dictionary<string, XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass> customInputs = [];
                    if (inputStatus.CustomInputPosition is not null)
                    {
                        customInputs.Add(
                            inputStatus.CustomInputPosition.ToString().ToCamelCase(),
                            new(inputStatus.InputState switch
                            {
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Healthy => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Ok,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Fault => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Fault,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Disabled,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Triggered => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Triggered,
                                _ => throw new InternalErrorException($"Unexpected input state specified. {inputStatus.CustomInputPosition} {inputStatus.InputState}")
                            }));
                    }
                    if (!string.IsNullOrEmpty(inputStatus.VendorSpecificCustomInputPosition))
                    {
                        customInputs.Add(
                            inputStatus.VendorSpecificCustomInputPosition.ToCamelCase(),
                            new(inputStatus.InputState switch
                            {
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Healthy => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Ok,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Fault => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Fault,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Disabled,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Triggered => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Triggered,
                                _ => throw new InternalErrorException($"Unexpected input state specified.{inputStatus.VendorSpecificCustomInputPosition} {inputStatus.InputState}")
                            }));
                    }

                    if (customInputs.Count > 0)
                    {
                        await StatusChangedEvent(new(
                            BanknoteNeutralization: new(
                                CustomInputs: customInputs
                            ))
                        );
                    }
                }
            }
            if (sender.GetType() == typeof(PowerManagementStatusClass) ||
                sender.GetType() == typeof(PowerManagementStatusClass.PowerInfoClass))
            {
                if (sender.GetType() == typeof(PowerManagementStatusClass))
                {
                    PowerManagementStatusClass powerManagementStatus = sender as PowerManagementStatusClass;
                    powerManagementStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (propertyInfo.PropertyName == nameof(powerManagementStatus.PowerSaveRecoveryTime) &&
                        powerManagementStatus.PowerSaveRecoveryTime < 0)
                    {
                        // power saving mode is not supported.
                        return;
                    }

                    await StatusChangedEvent(new(
                        PowerManagement: new(
                            PowerSaveRecoveryTime: propertyInfo.PropertyName != nameof(powerManagementStatus.PowerSaveRecoveryTime) ? null : powerManagementStatus.PowerSaveRecoveryTime
                            )
                        ));
                }
                else
                {
                    PowerManagementStatusClass.PowerInfoClass powerInfo = sender as PowerManagementStatusClass.PowerInfoClass;
                    powerInfo.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if ((propertyInfo.PropertyName == nameof(powerInfo.BatteryChargingStatus) &&
                         powerInfo.BatteryChargingStatus == PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.NotSupported) ||
                        (propertyInfo.PropertyName == nameof(powerInfo.BatteryStatus) &&
                         powerInfo.BatteryStatus == PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.NotSupported))
                    {
                        return;
                    }

                    await StatusChangedEvent(new(
                        PowerManagement: new(
                            Info: new(
                                PowerInStatus: propertyInfo.PropertyName != nameof(powerInfo.PowerInStatus) ?
                                null :
                                powerInfo.PowerInStatus switch
                                {
                                    PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.Powering,
                                    PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.NoPower,
                                    _ => throw new InternalErrorException($"Unexpected power-in status specified. {powerInfo.PowerInStatus}")
                                },
                                PowerOutStatus: propertyInfo.PropertyName != nameof(powerInfo.PowerOutStatus) ?
                                null :
                                powerInfo.PowerOutStatus switch
                                {
                                    PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.Powering,
                                    PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.NoPower,
                                    _ => throw new InternalErrorException($"Unexpected power-out status specified. {powerInfo.PowerOutStatus}")
                                },
                                BatteryStatus: propertyInfo.PropertyName != nameof(powerInfo.BatteryStatus) ?
                                null :
                                powerInfo.BatteryStatus switch
                                {
                                    PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Low => XFS4IoT.PowerManagement.BatteryStatusEnum.Low,
                                    PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Failure => XFS4IoT.PowerManagement.BatteryStatusEnum.Failure,
                                    PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Operational => XFS4IoT.PowerManagement.BatteryStatusEnum.Operational,
                                    PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Full => XFS4IoT.PowerManagement.BatteryStatusEnum.Full,
                                    PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Critical => XFS4IoT.PowerManagement.BatteryStatusEnum.Critical,
                                    _ => throw new InternalErrorException($"Unexpected battery status specified. {powerInfo.BatteryStatus}")
                                },
                                BatteryChargingStatus: propertyInfo.PropertyName != nameof(powerInfo.BatteryChargingStatus) ?
                                null :
                                powerInfo.BatteryChargingStatus switch
                                {
                                    PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.Charging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Charging,
                                    PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.NotCharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.NotCharging,
                                    PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.Discharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Discharging,
                                    _ => throw new InternalErrorException($"Unexpected battery charging status specified. {powerInfo.BatteryChargingStatus}")
                                }))
                        ));
                }
            }
            if (sender.GetType() == typeof(DepositStatusClass))
            {
                DepositStatusClass depositStatus = sender as DepositStatusClass;
                depositStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                await StatusChangedEvent(new(
                    Deposit: new(
                        DepTransport: propertyInfo.PropertyName != nameof(depositStatus.DepositTransport) ?
                        null :
                        depositStatus.DepositTransport switch
                        {
                            DepositStatusClass.DepositTransportEnum.Healthy => XFS4IoT.Deposit.StatusClass.DepTransportEnum.Ok,
                            DepositStatusClass.DepositTransportEnum.Inoperative => XFS4IoT.Deposit.StatusClass.DepTransportEnum.Inoperative,
                            DepositStatusClass.DepositTransportEnum.Unknown => XFS4IoT.Deposit.StatusClass.DepTransportEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected deposit transport status specified. {depositStatus.DepositTransport}")
                        },
                        EnvDispenser: propertyInfo.PropertyName != nameof(depositStatus.EnvelopDispenser) ?
                        null :
                        depositStatus.EnvelopDispenser switch
                        {
                            DepositStatusClass.EnvelopDispenserEnum.Healthy => XFS4IoT.Deposit.StatusClass.EnvDispenserEnum.Ok,
                            DepositStatusClass.EnvelopDispenserEnum.Inoperative => XFS4IoT.Deposit.StatusClass.EnvDispenserEnum.Inoperative,
                            DepositStatusClass.EnvelopDispenserEnum.Unknown => XFS4IoT.Deposit.StatusClass.EnvDispenserEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected envelop dispenser status specified. {depositStatus.EnvelopDispenser}")
                        },
                        Printer: propertyInfo.PropertyName != nameof(depositStatus.Printer) ?
                        null :
                        depositStatus.Printer switch
                        {
                            DepositStatusClass.PrinterEnum.Healthy => XFS4IoT.Deposit.StatusClass.PrinterEnum.Ok,
                            DepositStatusClass.PrinterEnum.Inoperative => XFS4IoT.Deposit.StatusClass.PrinterEnum.Inoperative,
                            DepositStatusClass.PrinterEnum.Unknown => XFS4IoT.Deposit.StatusClass.PrinterEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected printer status specified. {depositStatus.Printer}")
                        },
                        Toner: propertyInfo.PropertyName != nameof(depositStatus.Toner) ?
                        null :
                        depositStatus.Toner switch
                        {
                            DepositStatusClass.TonerEnum.Full => XFS4IoT.Deposit.StatusClass.TonerEnum.Full,
                            DepositStatusClass.TonerEnum.Low => XFS4IoT.Deposit.StatusClass.TonerEnum.Low,
                            DepositStatusClass.TonerEnum.Out => XFS4IoT.Deposit.StatusClass.TonerEnum.Out,
                            DepositStatusClass.TonerEnum.Unknown => XFS4IoT.Deposit.StatusClass.TonerEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected toner status specified. {depositStatus.Toner}")
                        },
                        Shutter: propertyInfo.PropertyName != nameof(depositStatus.Shutter) ?
                        null :
                        depositStatus.Shutter switch
                        {
                            DepositStatusClass.ShutterEnum.Closed => XFS4IoT.Deposit.StatusClass.ShutterEnum.Closed,
                            DepositStatusClass.ShutterEnum.Open => XFS4IoT.Deposit.StatusClass.ShutterEnum.Open,
                            DepositStatusClass.ShutterEnum.Jammed => XFS4IoT.Deposit.StatusClass.ShutterEnum.Jammed,
                            DepositStatusClass.ShutterEnum.Unknown => XFS4IoT.Deposit.StatusClass.ShutterEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected shutter status specified. {depositStatus.Shutter}")
                        },
                        DepositLocation: propertyInfo.PropertyName != nameof(depositStatus.DepositLocation) ?
                        null :
                        depositStatus.DepositLocation switch
                        {
                            DepositStatusClass.DepositLocationEnum.None => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.None,
                            DepositStatusClass.DepositLocationEnum.Container => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Container,
                            DepositStatusClass.DepositLocationEnum.Shutter => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Shutter,
                            DepositStatusClass.DepositLocationEnum.Unknown => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Unknown,
                            DepositStatusClass.DepositLocationEnum.Transport => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Transport,
                            DepositStatusClass.DepositLocationEnum.Removed => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Removed,
                            DepositStatusClass.DepositLocationEnum.Printer => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Printer,
                            _ => throw new InternalErrorException($"Unexpected deposit location status specified. {depositStatus.DepositLocation}")
                        })
                    ));
            }
        }
    }
}
