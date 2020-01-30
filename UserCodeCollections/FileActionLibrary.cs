using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Security;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Tesing;

namespace RanorexLibrary.UserCodeCollections
{
    /// <summary>
    /// File Contorol Library
    /// </summary>
    [UserCodeCollection]
    public static class FileActionsLibrary
    {
        /// <summary>
        /// compair files bynary
        /// if setting argument is same Path, validate Failed
        /// </summary>
        /// <param name="FilePath">FilePath</param>
        /// <param name="CompairsonFilePath">CompairsonFilePath</param>
        public static void FileCompair(string FilePath, string CompairsonFilePath)
        {
            Report.Log(ReportLevel.Info, 
                "User Code" 
                , String.Format("compair File({0}) to CompaiersonFile({1})" // can't use interpolated   
                    , FilePath 
                    , CompairsonFilePath)
                , null);
                
            // guard
            if (FilePath == String.Empty) 
            {
                Validate.Fail("Empty FilePath");
            }           
            if (CompairsonFilePath == String.Empty) 
            {
                Validate.Fail("Empty CompairsonFilePath");
            } 
            if (FilePath == CompairsonFilePath)
            {
                Validate.Fail(String.Format(" FilePath({0}) is same path from CompairsonFilePath({1})"
                                                , FilePath
                                                , CompairsonFilePath));
            }

            int FileByte;
            int CompairsonFileByte;
            FileStream FileStream;
            FileStream CompairsonFileStream;

            try
            {
                using(FileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                using(FileStream = new FileStream(CompairsonFilePath, FileMode.Open, FileAccess.Read))
                {
                    if(FileStream.Length != CompairsonFilePath.Length)
                    {
                        Validate.Fail(String.Format(" FilePath({0}) is different from CompairsonFilePath({1})"
                                                , FilePath
                                                , CompairsonFilePath));
                    }

                    do
                    {
                        FileByte = FileStream.ReadByte();
                        CompairsonFileByte = CompairsonFileStream.ReadByte();
                    }
                    // if FileByte is diffrent from CompairsonFileByte or reach end byte, stop roop
                    while((FileByte == CompairsonFileByte) && (FileByte != -1));
                }

                // if tow files reached end, match files.  FileByte - ComapairsonFileByte = 0
                // if FileByte is diffrent from CompairsonFileByte, FileByte - ComapairsonFileByte != 0
                Validate.IsTrue( ((FileByte - CompairsonFileByte) == 0)
                                , String.Format("validate compair files(File({0}, CompairsonFile({1})))"
                                    , FilePath
                                    , CompairsonFilePath));
            } catch(ArgumentException arEx) {
                Validate.Fail("incorrect FilePath");
                System.Console.WriteLine(arEx.StackTrace);
            } catch(SecurityException seEx) {
                Validate.Fail("can't access FilePath. check access authority");
                System.Console.WriteLine(seEx.StackTrace);
            } catch(IOException ioEx) {
                Validate.Fail("failed get FileData, rerun this test case");
                System.Console.WriteLine(ioEx.StackTrace);
            }
        }
    }

}