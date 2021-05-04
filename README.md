# KAL-XFS4IoT-SP-Dev

KAL's XFS4IoT SP-Dev is an open source implementation of the revolutionary new XFS version 4 global standard. It is ready for the IoT era and paves the way for a cloud-based, secure, OS-agnostic ATM industry.

This pioneering framework will enable hardware manufacturers and vendors to quickly create XFS4IoT SPs and offers numerous benefits. As well as being OS-agnostic, it is multivendor and works with hardware from multiple vendors, providing banks with greater choice, enhanced security and lower costs.

Hardware and software vendors, test software suppliers and application developers are all invited to join a working group to help develop the new framework, which will be available under the MIT open source license.

Find out more about this exciting new project and how you can get involved in shaping the ground-breaking XFS4IoT SP-Dev framework by contacting us @ xfs4iot_sp-dev_info@kal.com

## Building the sample simulator

1. Clone KAL_XFS4IoT_SP-Dev repository from https://github.com/KAL-ATM-Software/KAL_XFS4IoT_SP-Dev.
2. Install Visual Studio 2019 and .Net5 SDK or runtime.
3. Open the solution `Devices/SPs.sln` in Visual Studio 2019.
4. Select Solution SPs in the solution explorer and execute Clean Solution.
5. Select Solution SPs in the solution explorer and execute Rebuild Solution.
6. Build complete all 7 projects without errors.
7. Binaries are created under `Devices/bin/Debug/net5.0-windows7.0` or `Release/net5.0-windows7.0`.
8. The SP executable **XFS4IoT.SP.ServerHostSample.exe** is created. The executable has dependencies with other DLLs created in the same folder.
9. The default IP address is the local host 127.0.0.0. If the SP and client application run on the same machine, no configuration changes are required. If the IP address needs to be changed it can be done so remotely with the client application from different machine as follows: 
    1. Open the configuration file of the SP, ***XFS4IoT.SP.ServerHostSample.dll.config***, in the test file editor, for example VS Code.
    2. Change value of the `ServerAddress` key.  
        `<add key="ServerAddressUri" value="**http://xxx.xxx.xxx.xxx**" />`
10. Run XFS4IoT.SP.ServerHostSample.exe.

## Building the sample client application

1. Clone KAL_XFS4IoT_SP-Dev repository from https://github.com/KAL-ATM-Software/KAL_XFS4IoT_SP-Dev.
2. Install Visual Studio 2019 and .Net5 SDK or runtime.
3. Open the solution `ClientTestApp/ClientTestApp.sln` in Visual Studio 2019.
4. Select ClientTestApp in the solution explorer and execute Clean Solution.
5. Select ClientTestApp in the solution explorer and execute Rebuild Solution.
6. Build complete all 4 projects without errors.
7. Binaries are created under `ClientTestApp/bin/Debug/net5.0-windows` or `Release/net5.0-windows`.
8. The SP executable **TestClientForms.exe** is created. The executable has dependencies with other DLLs created in the same folder.
9. Run **TestClientForms.exe**
10. The default IP address is the local host 127.0.0.1. If the SP and client application run on the same machine, no configuration changes are required.  
If the SP and client application run on the different machines, change the Service URL on the top left of GUI. i.e. **ws://xxx.xxx.xxx.xxx** where the SP is running.
11. Click the Service Discovery button.  
The CardReader URL is displayed if the connection is established with the SP.
12. The test application can communicate with the CardReader SP.
