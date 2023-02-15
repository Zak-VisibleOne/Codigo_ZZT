using POSData;
using System.Web;
using System.Web.Mvc;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity.Validation;
using System.IO;
using POSSystem.Models;
using POSSystem.Utils;
using static POSSystem.Models.ViewModel;

namespace POSSystem.Controllers
{
    public class MediaController : Controller
    {
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public string SQLPath = "Media/";
        private POSSystemEntities Context { get; set; }
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MediaController));

        public MediaController()
        {
            Context = new POSSystemEntities();
        }
        // GET: Media
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult Display()
        {
            return View();
        }
        public class MediaImageListView
        {
            public Int64 SrNo { get; set; }
            public int ID { get; set; }
            public string MediaName_enUS { get; set; }
            public string MediaUrl_enUS { get; set; }
        }
        public JsonResult GetMediaImageListing()
        {
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            var list = sd.GetDataByQuery<MediaImageListView>(SQLPath + "GetMediaList", new object[] { }).ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (MediaImageListView l in list)
                {
                    string fileName = l.MediaUrl_enUS;
                    FileInfo fi = new FileInfo(fileName);
                    string extn = fi.Extension;
                    string imageUrl = "";
                    if (extn == ".pdf")
                    {
                        imageUrl = "/MediaFolder/sample/pdfsample.png";
                    }
                    else if (extn == ".exe")
                    {
                        imageUrl = "/MediaFolder/sample/imgexe.png";
                    }
                    else
                    {
                        imageUrl = l.MediaUrl_enUS ?? "";
                    }
                    data.Add(new object[] {
                         l.SrNo
                        , l.MediaName_enUS
                        , imageUrl
                        , ""
                        , l.ID
                    });
                }
            }
            return Json(
                    new
                    {
                        recordsTotal = list_count,
                        recordsFiltered = list_count,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult Get_MediaSetImageList()
        {
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            List<MediaImageListView> list = new List<MediaImageListView>();
            list = sd.GetDataByQuery<MediaImageListView>(SQLPath + "GetMediaList", new object[] { }).ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (MediaImageListView l in list)
                {
                    string fileName = l.MediaUrl_enUS;
                    FileInfo fi = new FileInfo(fileName);
                    //// Get File Name  
                    //string justFileName = fi.Name;           
                    //// Get file name with full path   
                    //string fullFileName = fi.FullName;              
                    //// Get file extension   
                    string extn = fi.Extension;
                    //Console.WriteLine("File Extension: {0}", extn);
                    string imageUrl = "";
                    if (extn == ".pdf")
                    {
                        imageUrl = "/MediaFolder/sample/pdfsample.png";
                    }
                    else if (extn == ".exe")
                    {
                        imageUrl = "/MediaFolder/sample/exe.png";
                    }
                    else
                    {
                        imageUrl = l.MediaUrl_enUS ?? "";
                    }
                    data.Add(new object[] {
                         l.SrNo
                        ,imageUrl
                        ,l.MediaName_enUS
                        ,""
                        ,imageUrl
                        ,l.ID
                    });
                }
            }
            return Json(
                    new
                    {
                        recordsTotal = list_count,
                        recordsFiltered = list_count,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                POSSystemEntities context = new POSSystemEntities();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/MediaFolder/" + ImageName);
                file.SaveAs(physicalPath);
                MediaInfo media = new MediaInfo();
                media.MediaName_enUS = Request.Form["MediaName"];
                media.MediaTypeName = "Image";
                media.MediaUrl_enUS = ImageName;
                media.MediaType = 1;
                media.CreatedDate = DateTime.Now;
                media.LastEdited = DateTime.Now;
                media.Width = 1;
                media.Height = 1;
                media.Size = "";
                media.Type = "";
                context.MediaInfoes.Add(media);
                context.SaveChanges();
            }
            return RedirectToAction("../Media/Media/");
        }
        [HttpPost]
        public JsonResult UploadMediaFile()
        {
            ResultMessage result = new ResultMessage();
            string _imgname = string.Empty;
            string locFile = string.Empty;
            string contentType = string.Empty;
            string length = string.Empty;
            string name = string.Empty;
            string extension = string.Empty;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            name = Path.GetFileNameWithoutExtension(file.FileName);
                            fname = file.FileName;
                            contentType = file.ContentType;
                            int contentLength = file.ContentLength;
                            extension = Path.GetExtension(fname);
                            length = SizeSuffix(contentLength);
                            _imgname = fname;
                        }
                        locFile = "~/MediaFolder/MediaPage";
                        bool exists = Directory.Exists(Server.MapPath(locFile));
                        if (!exists)
                        {
                            Directory.CreateDirectory(Server.MapPath(locFile));
                        }
                        fname = Path.Combine(Server.MapPath(locFile), fname);
                        if (System.IO.File.Exists(fname))
                        {
                            string newfilename = "";
                            name = name + "_" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "";
                            newfilename = name + "_" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "" + extension;
                            _imgname = newfilename;
                            fname = Path.Combine(Server.MapPath(locFile), newfilename);
                        }
                        file.SaveAs(fname);
                        using (var context = new POSSystemEntities())
                        {
                            MediaInfo media = new MediaInfo();
                            media.MediaName_enUS = name;
                            media.MediaTypeName = contentType;
                            media.MediaUrl_enUS = "/MediaFolder/MediaPage/" + _imgname;
                            media.MediaType = 1;
                            media.CreatedDate = DateTime.Now;
                            media.LastEdited = DateTime.Now;
                            media.Width = 1;
                            media.Height = 1;
                            media.Size = length;
                            media.Type = "";
                            context.MediaInfoes.Add(media);
                            context.SaveChanges();
                        }
                    }
                    _imgname = "/MediaFolder/MediaPage/" + _imgname + "";
                    result.message = _imgname;
                    result.data = contentType;
                    var list = new List<string> { length, name };
                    result.datalist = list;
                    result.result = "success";
                    string user = (Session["User"] ?? "").ToString();
                    logger.Info("System user: " + user + " update image with image name " + name + ".");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public JsonResult GetMediaList()//int ContentID, int PageID, int PageDetailID
        {
            return Json(MediaList(), JsonRequestBehavior.AllowGet);
        }
        public List<MediaInfo> MediaList()
        {
            List<MediaInfo> lst = new List<MediaInfo>();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SelectMediaInfo", con);
                //cmd.Parameters.Add("@ContentID", SqlDbType.Int).Value = ContentID;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MediaInfo
                    {
                        ID = Convert.ToInt32(rdr["ID"]),
                        MediaName_enUS = rdr["MediaName_enUS"].ToString(),
                        MediaUrl_enUS = rdr["MediaUrl_enUS"].ToString(),
                    });
                }
                return lst;
            }
        }
        public JsonResult DeleteMediaByID(int MediaID)
        {
            ResultMessage result = new ResultMessage();
            result.result = "success";
            result = DeleteMedia(MediaID);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ResultMessage DeleteMedia(int id)
        {
            ResultMessage rm = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var mediaObj = context.MediaInfoes.Where(x => x.ID == id).FirstOrDefault();
                if (mediaObj != null)
                {
                    string FilePath = Server.MapPath((mediaObj.MediaUrl_enUS ?? ""));
                    if (System.IO.File.Exists(FilePath))
                    {
                        try
                        {
                            System.IO.File.Delete(FilePath);
                        }
                        catch (System.IO.IOException e)
                        {
                        }
                    }
                }
                context.Database.ExecuteSqlCommand(@"delete ace_report.MediaInfo where ID = {0}", new object[] { id });
                context.SaveChanges();
                //dbContextTransaction.Commit();
                rm.message = "Data successfully deleted";
                rm.result = "success";
                string user = (Session["User"] ?? "").ToString();
                logger.Info("System user: " + user + " delete media with id no " + id + ".");
            }
            return rm;
        }
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }
            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
        public JsonResult MediaByID(int MediaID)
        {
            var data = GetMediaByID(MediaID);
            if (data != null)
            {
                if (data.MediaUrl_enUS == "" || data.MediaUrl_enUS == null)
                {
                    data.MediaUrl_enUS = "\\MediaFolder\\user-6.jpg";
                }
                FileInfo fi = new FileInfo(data.MediaUrl_enUS);
                string extn = fi.Extension;
                if (extn == ".pdf")
                {
                    data.MediaUrl_enUS = "/MediaFolder/sample/pdfsample.png";
                }
                else if (extn == ".exe")
                {
                    data.MediaUrl_enUS = "/MediaFolder/sample/exe.png";
                }
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public ViewModelMedia GetMediaByID(int mediaID)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ViewModelMedia>("Media/GeMediaByID", new object[] { mediaID }).FirstOrDefault();
            return result;
        }
    }
}