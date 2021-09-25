using System;
namespace DianaScript
{
    public class App
    {
        public static void Main(string [] args)
        {
            var parser = new AIRParser(args[0]);
            var code = parser.ReadCode();
            var vm = new VM();
            vm.Run(code);
        }
    }
}