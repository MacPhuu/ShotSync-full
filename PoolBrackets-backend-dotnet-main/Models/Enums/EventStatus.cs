namespace PoolBrackets_backend_dotnet.Models.Enums
{
    public enum EventStatus
    {
        Draft = 0,             // Mới tạo, đang cho đăng ký VĐV
        DoubleElimination = 1, // Đang diễn ra vòng sơ loại (2 mạng)
        SingleElimination = 2, // Đang diễn ra vòng chung kết (Knock-out)
        Finished = 3,          // Đã kết thúc
        Cancelled = 4          // Đã hủy
    }
}