using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    class MyException : Exception
    {
        public MyException( string message ) : base( message )
        {

        }

        public MyException(string message, Exception inner) : base(message) { }
    }

    class InvalidNakiException : Exception
    {
        public InvalidNakiException(string message) : base(message){}

        public InvalidNakiException(string message, Exception inner) : base(message) { }
    }
}
