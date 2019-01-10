using GASDBLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace GASLibrary.Tests
   
{
     public class LoginProcessTests
    {
           [Theory]
           [InlineData ("j","c",true)]
           [InlineData ("jjj","ppp", true)]
           [InlineData ("1","password",false)]
           [InlineData ("","",false)]
           [InlineData ("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq","password",false)]
           [InlineData ("user","qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq",false)]
           [InlineData ("-user","password", false)]
            public void ValidateUserInput_StringsShouldVerify(string username, string password, bool expected)
            {
                //Arrange

                LogonProcesses lp = new LogonProcesses();
                //bool expected = true;

                //Act

                bool actual = lp.ValidateUserInput(username,password);

            //Assert

            Assert.Equal(actual, expected);

     
        }
    }
}
