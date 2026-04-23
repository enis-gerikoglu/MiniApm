namespace MiniApm.Consumer.Models;

public class SystemMetricsMessage
{
    public string MachineName { get; set; } = default!;
    public string Os { get; set; } = default!;
    public double CpuUsage { get; set; }
    public int CpuCoreCount { get; set; }
    public double MemoryUsage { get; set; }
    public double TotalMemory { get; set; }
    public double AvailableMemory { get; set; }
    public double UsedMemory { get; set; }
    public double DiskUsage { get; set; }
    public double TotalDisk { get; set; }
    public double FreeDisk { get; set; }
    public double UsedDisk { get; set; }
    public DateTime Timestamp { get; set; }

}