﻿using System;
using System.Collections.Generic;

namespace BTL_LapTrinhWeb.Data;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? MatKhau { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
