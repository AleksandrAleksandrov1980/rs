using System.Runtime.InteropServices;
namespace aval;
public class CRwrapper
{
    [DllImport("/home/ustas/projects/git_r/rastr/RastrWin/astra/build/libastra_shrd.so")]
    private static extern int test(); 

    public int call_test()
    {
        int nRes =0 ;
        nRes = test();
        return nRes;
    }

}