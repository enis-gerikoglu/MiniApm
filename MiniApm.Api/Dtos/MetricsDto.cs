namespace MiniApm.Api.Dtos;

public class MetricsDto
{
    public string MachineName { get; set; } = default!;
    public double CpuUsage { get; set; }
    public double MemoryUsage { get; set; }
    public double DiskUsage { get; set; }
    public DateTime Timestamp { get; set; }
}