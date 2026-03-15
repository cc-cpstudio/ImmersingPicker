using System.Management;
using System.Runtime.InteropServices;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Services.Services.Security;

public class UsbDriveService
{
    public static List<UsbDriveInfo> GetUsbDrives()
    {
        var drives = new List<UsbDriveInfo>();
        try
        {
            var driveInfos = DriveInfo.GetDrives()
                .Where(d => d is { DriveType: DriveType.Removable, IsReady: true });
            foreach (var drive in driveInfos)
            {
                drives.Add(new UsbDriveInfo
                {
                    VolumeSerialNumber = GetVolumeSerialNumber(drive.Name),
                    DriveLetter = drive.Name,
                    VolumeLabel = drive.VolumeLabel
                });
            }
        }
        catch (Exception ex)
        {
            // TODO
        }
        return drives;
    }

    public static UsbDriveInfo? FindMatchingUsbDrive(UsbDriveInfo targetDrive)
    {
        var currentDrives = GetUsbDrives();
        foreach (var drive in currentDrives)
        {
            if (drive == targetDrive)
            {
                return drive;
            }
        }
        return null;
    }

    private static string GetVolumeSerialNumber(string driveLetter)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return string.Empty;
        try
        {
            var searcher = new ManagementObjectSearcher(
                $"SELECT VolumeSerialNumber FROM Win32_LogicalDisk WHERE DeviceID = '{driveLetter.TrimEnd('\\')}'"
            );
            foreach (var obj in searcher.Get())
            {
                return obj["VolumeSerialNumber"]?.ToString() ?? string.Empty;
            }
        }
        catch
        {
            return string.Empty;
        }
        return string.Empty;
    }
}