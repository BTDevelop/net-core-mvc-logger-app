using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticLoggerApp.Models
{
    public enum EnumLogState
    {
        Update = 1,
        Delete = 2,
        Insert = 3
    }

    public class ModifyLog
    {
        public string EntityName { get; set; }  //tablo adı
        public string PropertyName { get; set; } //kayıt adı
        public int PrimaryKeyColNameVal { get; set; } //sütun adı
        public string OriginVal { get; set; } //ilk değeri
        public string NewVal { get; set; } //son değeri
        public DateTime LogDate { get; set; } //zamanı
        public EnumLogState State { get; set; } //durumu
    }
}
