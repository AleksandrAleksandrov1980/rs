using System.Runtime.InteropServices;
namespace aval;
public class CRwrapper
{
    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so")]
    private static extern int test(); 
    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so")]
    private static extern long RastrCreate();
    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so", CharSet = CharSet.Ansi)]
    private static extern long Load( long idRastr, string  pch_fpath, string  pch_tpath );

    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so", CharSet = CharSet.Ansi)]
    private static extern long Rgm( long idRastr, string pch_parameters );

    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so", CharSet = CharSet.Ansi)]
    private static extern long SetValInt( long idRastr, string pch_table, string pch_col, long n_row, long n_val );
    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so", CharSet = CharSet.Ansi)]
    private static extern long GetValInt( long idRastr, string pch_table, string pch_col, long n_row, ref long n_val_out );

    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so", CharSet = CharSet.Ansi)]
    private static extern long GetValDbl( long idRastr, string pch_table, string pch_col, long n_row, ref double d_val_out );

    public long call_test()
    {
        long nRes =0 ;
        
        nRes = (int)test();

        long idRastr = -1;
        idRastr = RastrCreate();
        nRes = Load( idRastr, "/home/ustas/projects/test-rastr/Metro/2023_06_28/d1", "" );
        nRes = Rgm( idRastr, "p1" );
        nRes = SetValInt( idRastr, "node", "na", 0, 1 );
        long ll = 0;
        nRes = GetValInt( idRastr, "node", "na", 0, ref  ll );
        double dd = 0;
        nRes = GetValDbl( idRastr, "node", "pg", 0, ref dd );

        return nRes;
    }

}