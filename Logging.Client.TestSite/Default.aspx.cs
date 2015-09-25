using System;
using System.Collections.Generic;

namespace Logging.Client.TestSite
{
    public partial class Default : System.Web.UI.Page
    {

        readonly static ILog logger = LogManager.GetLogger(typeof(Default));
        readonly static ILog logger2 = LogManager.GetLogger("Default2");
        protected void Page_Load(object sender, EventArgs e)
        {


            //for (int i = 0; i < 100; i++)
            //{
//            logger.Debug("test");
//            logger.Info("aabbbbbcc", "test");
//            logger.Warm("test", "大大的打算打算大大", null);



//            Dictionary<string, string> tags = new Dictionary<string, string>();
//            tags.Add("a", "a");
//            tags.Add("adsadadad", "aweqweqweqe2312313gdgdfgdg!@##$%");
//            tags.Add("a阿迪达sad", "aweqweqweq打啊打多大的萨达大厦e2312313gdgdfgdg!@##$%");
//            tags.Add("a阿迪达2424sad", @"aweqweqweq打啊打多大的萨达大厦e2312313gdgdfgdga
//                weqweqweq打啊打多大的萨达大厦e2312313gdgdfgdgaweqweqweq打啊打多大的
//                萨达大厦e2312313gdgdfgdgaweqwwrqwq付费方式是放松放
//                萨达大厦e2312313gdgdfgdgaweqwwrqwq付费方式是放松放
//                萨达大厦e2312313gdgdfgdgaweqwwrqwq付费方式是放松放
//                萨达大厦e2312313gdgdfgdgaweqwwrqwq付费方式是放松放
//                松放松放松放松eqweq打啊打多大的萨达大厦e2312313gdgdfgdg!@##$%");
//            logger2.Error("test", "test", tags);

//            Exception ex1 = new Exception("Exception test 1");
//            Exception ex2 = new Exception("Exception test 2",ex1);
//            Exception ex3 = new Exception("Exception test 3",ex2);
//            Exception ex4 = new Exception("Exception test 4",ex3);
//            Exception ex5 = new Exception("Exception test 5",ex4);


//            logger2.Error(ex5);
            throw new Exception("dada");
            //}



            //try
            //{
            //    throw new Exception("test exception");
            //}
            //catch(Exception ex)
            //{
            //    logger.Error(ex);
            //}



        }
    }
}