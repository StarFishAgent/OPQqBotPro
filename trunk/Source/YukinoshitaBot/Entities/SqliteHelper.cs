using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace YukinoshitaBot.Entities
{
    public static class SqliteHelper
    {
        static string strConn = "Data Source=D:\\git\\new\\MyProject\\GitHub\\C#\\GenshinBot.git\\trunk\\GenshinBotCore\\GenshinBot.db";
        public static DataTable EQ(string StrSql)
        {
            var dt = new DataTable();
            var conn = new SQLiteConnection(strConn);
            new SQLiteDataAdapter(StrSql, conn).Fill(dt);
            return dt;
        }
        public static void ENQ(string StrSql)
        {
            var conn = new SQLiteConnection(strConn);
            var cmd = new SQLiteCommand(StrSql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
