using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbumImages
{
    public class GetAlbumImagesCommand : CommandBase, ICommand<Guid, List<string>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public List<string> Execute(Guid request)
        {
            if (request.ToString().ToUpper() == "A656559E-25FB-4D19-B2B4-A24400FD17F1")
            {
                return new List<string>
                           {
                               "http://bettercms.sandbox.mvc4.local/uploads/image/c3ff46d589054bcfbbbe27358fae9788/WallsFunnyCatsPack10_03_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/990425bed04042acb9b707fb8d0fcb82/WallsFunnyCatsPack10_02_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/655330d3d91d44e597021d5761ebee29/WallsFunnyCatsPack10_01_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/0c3924ddf4c74ed29922b7b54db87ca4/WallsFunnyCatsPack10_04_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/dbbbb519bcb94450b9b2815a1132118d/WallsFunnyCatsPack10_05_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/4471ce53683441478a77bdcda4480f49/WallsFunnyCatsPack10_06_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/ef4f954e90054e2c9a88072b2e5fccb5/Single'Quote_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/c2ec1f9731fe4fc19bca09ce20ed6ab3/WallsFunnyCatsPack10_08_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/c65856bfa018417e985206fbbd45c242/WallsFunnyCatsPack10_07_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/882d8357dd1b41f08dad02747f2a4e6d/WallsFunnyCatsPack10_09_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/c2abf181645a402bbd9aa78185b5f3d0/WallsFunnyCatsPack10_10_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/dc1fbd679acb48c28cd38b7ac41b6a46/WallsFunnyCatsPack10_11_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/76860f5322574825944fc638a51d350c/WallsFunnyCatsPack10_12_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/e8a5a2595bd6473a830cc475ec6e5696/WallsFunnyCatsPack10_13_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/67617a88be9f446ba1f0e1a670727032/WallsFunnyCatsPack10_14_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/625d0a4f0cb54494aac5e65140ffce72/WallsFunnyCatsPack10_16_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/9521bab5a1e242cdbf974b1534c96751/WallsFunnyCatsPack10_15_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/6360e8684c0f47aa800721077a66ad4e/WallsFunnyCatsPack10_17_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/d640c50ee80747debaa0cc6ca945b427/WallsFunnyCatsPack10_18_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/d53eddd2dfbc41119d53f5e192e6d3d6/WallsFunnyCatsPack10_20_1.jpg",
                                "http://bettercms.sandbox.mvc4.local/uploads/image/31e9e927d16b4ff0b79ec4fca4934b30/WallsFunnyCatsPack10_19_1.jpg"
                           };
            }
            else
            {
                return new List<string>
                           {
                               "http://bettercms.sandbox.mvc4.local/uploads/image/5da1814bef894c9285e6df78cc2772d8/funny-pictures-panda-does-a-trick-and-falls_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/48010b01f69e4acc8471564b33730624/2012-08-30 12.30.26_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/3c45313bf47d4df188cb5b8db061f641/beautiful amazing zoo panda animal picture panda images_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/5cf19cb6467b4766ac4f33f89bbb0295/__kara_goucher_12_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/43ec0c165b7949418da51ca7a0d2b7ed/little-panda-baby_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/d81a00c30c8647de9cfd7a8361d0cd72/panda-bear-eating_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/22c015daaa9945959178f8bc3f135ee0/Sweet-Panda-pandas-12538504-1600-1200_1.jpg",
"http://bettercms.sandbox.mvc4.local/uploads/image/eb077067532345cab0b57bb0278296d6/Grosser_Panda_1.JPG",
                           };
                
            }

            
        }
    }
}