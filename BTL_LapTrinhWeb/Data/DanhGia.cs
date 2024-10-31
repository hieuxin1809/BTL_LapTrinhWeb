using System;
using System.Collections.Generic;

namespace BTL_LapTrinhWeb.Data;

public partial class DanhGia
{
    public int Stt { get; set; }

    public int MaHh { get; set; }

    public string MaKh { get; set; } = null!;

    public string? Email { get; set; }

    public string BinhLuan { get; set; } = null!;

    public int? Rating { get; set; }

    public DateTime? Date { get; set; }

    public virtual HangHoa MaHhNavigation { get; set; } = null!;
}
