using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.Lambda.Core;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon;

namespace ImageBucket.Controllers
{
    [Route("v1/bucket")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        [HttpGet]
       
        public IActionResult GetImageText()
        {
            string photo = "image_2020_03_09T10_13_27_425Z.png";
            string bucket = "imagebucket080304";
            string imageText = "";

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(RegionEndpoint.USEast2);

            DetectTextRequest detectTextRequest = new DetectTextRequest()
            {
                Image = new Image()
                {
                    S3Object = new S3Object()
                    {
                        Name = photo,
                        Bucket = bucket
                    }
                }
            };
            try
            {
                 var  detectTextResponse =   rekognitionClient.DetectTextAsync(detectTextRequest);
                 var imageResponse = detectTextResponse.Result;
                 var image = imageResponse.TextDetections.Where(te => te.Type == "Line" && te.Confidence > 90).OrderByDescending(t=>t.Confidence).FirstOrDefault();
                 if(image!=null)
                {
                    imageText = image.DetectedText;
                }
            }
            catch (Exception e)
            {
                imageText = $"exception occured { e.Message }";
            }


            return Ok(imageText);
        }
    }
}