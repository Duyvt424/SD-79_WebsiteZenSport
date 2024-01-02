using AppAPI.DTO;
using AppAPI.Interfaces;
using AppData.Models;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AppAPI.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly ShopDBContext _dbContext;
        public ProductRepository(ShopDBContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<ProductDTO> FilterByColor(List<string> instanceColor)
        {
            ProductDTO respro = new ProductDTO();
            try
            {
                var shoeList = _dbContext.ShoesDetails.AsQueryable();

                var productList = _dbContext.Products.AsQueryable();
                respro.Shoe_Details = new List<shoesDetailsDTO>();
                foreach (var colorcode in instanceColor)
                {
                    var query = (from shoe in shoeList

                                 join color in _dbContext.Colors.AsQueryable()
                                 on shoe.ColorID equals color.ColorID into Color
                                 from col in Color.DefaultIfEmpty()

                                 join prroductbl in productList
                                 on shoe.ProductID equals prroductbl.ProductID into Product
                                 from prod in Product.DefaultIfEmpty()

                                 where col.Name == colorcode
                                 select new
                                 {
                                     shoe.ShoesDetailsId,
                                     shoe.ShoesDetailsCode,
                                     shoe.DateCreated,
                                     shoe.Price,
                                     shoe.ImportPrice,
                                     shoe.ProductID,
                                     shoe.Description,
                                     shoe.Status,
                                     shoe.ColorID,
                                     shoe.StyleID,
                                     shoe.ImageUrl,
                                     prod.Name,
                                     prod.SupplierID,
                                     shoe.SexID
                                 });
                    if (query.ToList().Count() > 0)
                    {
                        foreach (var item in query)
                        {
                            shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                            shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                            shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                            shoesDetails.DateCreated = item.DateCreated;
                            shoesDetails.Price = item.Price;
                            shoesDetails.ImportPrice = item.ImportPrice;
                            shoesDetails.ProductID = item.ProductID;
                            shoesDetails.Description = item.Description;
                            shoesDetails.Status = item.Status;
                            shoesDetails.ColorID = item.ColorID;
                            shoesDetails.StyleID = item.StyleID;
                            shoesDetails.ImageUrl = item.ImageUrl;
                            shoesDetails.SexID = item.SexID;
                            shoesDetails.Name = item.Name;
                            shoesDetails.SupplierID = item.SupplierID;


                            respro.Shoe_Details.Add(shoesDetails);
                        }
                    }
                }
                respro.status = 200;
                respro.Message = "Get Products Successfully!";
                return respro;
            }
            catch (Exception ex)
            {
                respro.Message = ex.Message;
                respro.status = 400;
             
                return respro;
            }
        }

        public async Task<ProductDTO> FilterBySex(List<string> instanceSex)
        {
            ProductDTO respro = new ProductDTO();
            try
            {
                var shoelist = _dbContext.ShoesDetails.AsQueryable();
                var productList = _dbContext.Products.AsQueryable();

                respro.Shoe_Details = new List<shoesDetailsDTO>();
                foreach (var sex in instanceSex)
                {
                    var query = (from shoe in shoelist

                                 join sexItem in _dbContext.Sex.AsQueryable()
                                 on shoe.SexID equals sexItem.SexID into Sex
                                 from se in Sex.DefaultIfEmpty()

                                 join producttbl in productList
                                 on shoe.ProductID equals producttbl.ProductID into Product
                                 from product in Product.DefaultIfEmpty()

                                 where se.SexName == sex
                                 select new
                                 {
                                     shoe.ShoesDetailsId,
                                     shoe.ShoesDetailsCode,
                                     shoe.DateCreated,
                                     shoe.Price,
                                     shoe.ImportPrice,
                                     shoe.ProductID,
                                     shoe.Description,
                                     shoe.Status,
                                     shoe.ColorID,
                                     shoe.StyleID,
                                     shoe.ImageUrl,
                                     product.SupplierID,
                                     product.Name,
                                     shoe.SexID
                                 });
                    if (query.ToList().Count() > 0)
                    {
                        foreach (var item in query)
                        {
                            shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                            shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                            shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                            shoesDetails.DateCreated = item.DateCreated;
                            shoesDetails.Price = item.Price;
                            shoesDetails.ImportPrice = item.ImportPrice;
                            shoesDetails.ProductID = item.ProductID;
                            shoesDetails.Description = item.Description;
                            shoesDetails.Status = item.Status;
                            shoesDetails.ColorID = item.ColorID;
                            shoesDetails.StyleID = item.StyleID;
                            shoesDetails.ImageUrl = item.ImageUrl;
                            shoesDetails.SexID = item.SexID;
                            shoesDetails.SupplierID = item.SupplierID;
                            shoesDetails.Name = item.Name;

                            respro.Shoe_Details.Add(shoesDetails);
                        }
                    }
                }
                respro.status = 200;
                respro.Message = "Get Products Successfully!";
                return respro;
            }
            catch (Exception ex)
            {
                respro.Message = ex.Message;
                respro.status = 400;
                return respro;
            }
        }

        public async Task<ProductDTO> FilterByStyle(List<string> instanceStyle)
        {
            ProductDTO respro = new ProductDTO();
            try
            {
                var shoeList = _dbContext.ShoesDetails.AsQueryable();

                var productList = _dbContext.Products.AsQueryable();

                respro.Shoe_Details = new List<shoesDetailsDTO>();
                foreach (var sty in instanceStyle)
                {
                    var query = (from shoe in shoeList

                                 join style in _dbContext.Styles.AsQueryable()
                                 on shoe.StyleID equals style.StyleID into Style
                                 from style in Style.DefaultIfEmpty()

                                 join producttbl in productList
                                 on shoe.ProductID equals producttbl.ProductID into Product
                                 from product in Product.DefaultIfEmpty()

                                 where style.Name == sty

                                 select new
                                 {
                                     shoe.ShoesDetailsId,
                                     shoe.ShoesDetailsCode,
                                     shoe.DateCreated,
                                     shoe.Price,
                                     shoe.ImportPrice,
                                     shoe.ProductID,
                                     shoe.Description,
                                     shoe.Status,
                                     shoe.ColorID,
                                     shoe.StyleID,
                                     shoe.ImageUrl,
                                     product.Name,
                                     product.SupplierID,
                                     shoe.SexID
                                 });
                    if (query.ToList().Count() > 0)
                    {
                        foreach (var item in query)
                        {
                            shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                            shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                            shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                            shoesDetails.DateCreated = item.DateCreated;
                            shoesDetails.Price = item.Price;
                            shoesDetails.ImportPrice = item.ImportPrice;
                            shoesDetails.ProductID = item.ProductID;
                            shoesDetails.Description = item.Description;
                            shoesDetails.Status = item.Status;
                            shoesDetails.ColorID = item.ColorID;
                            shoesDetails.StyleID = item.StyleID;
                            shoesDetails.ImageUrl = item.ImageUrl;
                            shoesDetails.SexID = item.SexID;
                            shoesDetails.SupplierID = item.SupplierID;
                            shoesDetails.Name = item.Name;
                            respro.Shoe_Details.Add(shoesDetails);
                        }
                    }
                }
                respro.status = 200;
                respro.Message = "Get Products Successfully!";
                return respro;
            }
            catch (Exception ex)
            {
                respro.Message = ex.Message;
                respro.status = 400;
                return respro;
            }
        }


       
        public async Task<ProductDTO> FilterShoe(List<RangeItemcs> instancePrice, List<string> instacneBranch, List<string> instanceStyle, List<string> instanceSex, List<string> instanceColor)
        {
            ProductDTO respro = new ProductDTO();
            try
            {
                respro.Shoe_Details = new List<shoesDetailsDTO>();
                if (instancePrice.Count > 0)
                {
                    foreach (var item in FilterShoeDetail(instancePrice).Result.Shoe_Details)
                    {
                        respro.Shoe_Details.Add(item);

                    }

                } else if (instacneBranch.Count > 0)
                {
                    foreach (var item in FilterShoeByBranch(instacneBranch).Result.Shoe_Details)
                    {
                        respro.Shoe_Details.Add(item);

                    }
                }
                else if (instanceSex.Count > 0)
                {
                    foreach (var item in FilterBySex(instanceSex).Result.Shoe_Details)
                    {
                        respro.Shoe_Details.Add(item);
                    }
                }
                else if (instanceStyle.Count > 0)
                {
                    foreach (var item in FilterByStyle(instanceStyle).Result.Shoe_Details)
                    {
                        respro.Shoe_Details.Add(item);
                    }
                }
                else if(instanceColor.Count > 0)
                {
                    foreach(var item in FilterByColor(instanceColor).Result.Shoe_Details)
                    {
                        respro.Shoe_Details.Add(item);
                    }
                }
                else
                {

                    var subquery = (from shoe in _dbContext.ShoesDetails.AsQueryable()  

                              
                                 join protbl in _dbContext.Products.AsQueryable()
                                 on shoe.ProductID equals protbl.ProductID into Product
                                 from product in Product.DefaultIfEmpty()

                                 select new
                                 {
                                     shoe.ShoesDetailsId,
                                     shoe.ShoesDetailsCode,
                                     shoe.DateCreated,
                                     shoe.Price,
                                     shoe.ImportPrice,
                                     shoe.ProductID,
                                     shoe.Description,
                                     shoe.Status,
                                     shoe.ColorID,
                                     shoe.StyleID,
                                     shoe.ImageUrl,
                                     product.SupplierID,
                                     product.Name,
                                     shoe.SexID,
                                 });

                    var newArrange = new List<shoesDetailsDTO>();


                    foreach( var item in subquery.ToList())
                    {
                        shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                        shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                        shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                        shoesDetails.DateCreated = item.DateCreated;
                        shoesDetails.Price = item.Price;
                        shoesDetails.ImportPrice = item.ImportPrice;
                        shoesDetails.ProductID = item.ProductID;
                        shoesDetails.Description = item.Description;
                        shoesDetails.Status = item.Status;
                        shoesDetails.ColorID = item.ColorID;
                        shoesDetails.StyleID = item.StyleID;
                        shoesDetails.ImageUrl = item.ImageUrl;
                        shoesDetails.SexID = item.SexID;
                        shoesDetails.Name = item.Name;
                        shoesDetails.SupplierID = item.SupplierID;

                        newArrange.Add(shoesDetails);
                    }
                    respro.Shoe_Details.Clear();
                    respro.Shoe_Details.AddRange(newArrange);
                }

                var query = (from shoe in respro.Shoe_Details 
                             
                             join producttbl in _dbContext.Products.AsQueryable()
                             on shoe.ProductID equals producttbl.ProductID  into Products
                             from product in Products.DefaultIfEmpty()

                             select new
                             {
                                 shoe.ShoesDetailsId,
                                 shoe.ShoesDetailsCode,
                                 shoe.DateCreated,
                                 shoe.Price,
                                 shoe.ImportPrice,
                                 shoe.ProductID,
                                 shoe.Description,
                                 shoe.Status,
                                 shoe.ColorID,
                                 shoe.StyleID,
                                 shoe.ImageUrl,
                                 product.SupplierID,
                                 product.Name,
                                 shoe.SexID,
                             });
                respro.status = 200;
                respro.Message = "Get Products Successfully!";

                var newquery = new List<shoesDetailsDTO>();


                foreach(var item in query.Distinct())
                {
                    shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                    shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                    shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                    shoesDetails.DateCreated = item.DateCreated;
                    shoesDetails.Price = item.Price;
                    shoesDetails.ImportPrice = item.ImportPrice;
                    shoesDetails.ProductID = item.ProductID;
                    shoesDetails.Description = item.Description;
                    shoesDetails.Status = item.Status;
                    shoesDetails.ColorID = item.ColorID;
                    shoesDetails.StyleID = item.StyleID;
                    shoesDetails.ImageUrl = item.ImageUrl;
                    shoesDetails.SexID = item.SexID;
                    shoesDetails.Name = item.Name;
                    shoesDetails.SupplierID = item.SupplierID;

                    newquery.Add(shoesDetails);
                }
                var newList = new List<shoesDetailsDTO>();
                newList.AddRange(newquery);
                respro.Shoe_Details.Clear();
                respro.Shoe_Details.AddRange(newList);
                return respro;
            } catch (Exception ex)
            {
                respro.Message = ex.Message;
                respro.status = 400;
                return respro;
            }
        }

        public async Task<ProductDTO> FilterShoeByBranch(List<string> instacneBranch)
        {
            ProductDTO respro = new ProductDTO();
            try
            {
                var shoeList = _dbContext.ShoesDetails.AsQueryable();
                respro.Shoe_Details = new List<shoesDetailsDTO>();
                foreach (var branch in instacneBranch)
                {
                    var query = (from shoe in shoeList



                                 join protbl in _dbContext.Products.AsQueryable()
                                 on shoe.ProductID equals protbl.ProductID into Product
                                 from product in Product.DefaultIfEmpty()

                                 join supplier in _dbContext.Suppliers.AsQueryable()
                                 on product.SupplierID equals supplier.SupplierID into Supplier
                                 from sp in Supplier.DefaultIfEmpty()


                                 where sp.Name == branch
                                 select new
                                 {
                                     shoe.ShoesDetailsId,
                                     shoe.ShoesDetailsCode,
                                     shoe.DateCreated,
                                     shoe.Price,
                                     shoe.ImportPrice,
                                     shoe.ProductID,
                                     shoe.Description,
                                     shoe.Status,
                                     shoe.ColorID,
                                     shoe.StyleID,
                                     shoe.ImageUrl,
                                     sp.SupplierID,
                                     product.Name,
                                     shoe.SexID,
                                 });




                    if (query.Count() > 0)
                    {
                        foreach (var item in query.ToList())
                        {
                            shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                            shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                            shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                            shoesDetails.DateCreated = item.DateCreated;
                            shoesDetails.Price = item.Price;
                            shoesDetails.ImportPrice = item.ImportPrice;
                            shoesDetails.ProductID = item.ProductID;
                            shoesDetails.Description = item.Description;
                            shoesDetails.Status = item.Status;
                            shoesDetails.ColorID = item.ColorID;
                            shoesDetails.StyleID = item.StyleID;
                            shoesDetails.ImageUrl = item.ImageUrl;
                            shoesDetails.SexID = item.SexID;
                            shoesDetails.Name = item.Name;
                            shoesDetails.SupplierID = item.SupplierID;


                            respro.Shoe_Details.Add(shoesDetails);
                        }
                    }
                }
                respro.status = 200;
                respro.Message = "Get Products Successfully!";
                return respro;
            }catch ( Exception ex)
            {
                respro.Message = ex.Message;
                respro.status = 400;
                return respro;
            }
        }

        public async Task<ProductDTO> FilterShoeDetail(List<RangeItemcs> instancePrice)
       {
            ProductDTO respro = new ProductDTO();
            try
           {
                var shoeList = _dbContext.ShoesDetails.AsQueryable();
               
                respro.Shoe_Details = new List<shoesDetailsDTO>();
                foreach (var rangceitem in instancePrice)
                {
                    // get value from
                    
                    if (rangceitem.min == decimal.Parse(4000000.ToString()))
                    {
                        var query = (from shoe in shoeList

                                     join producttbl in _dbContext.Products.AsQueryable()
                                     on shoe.ProductID  equals producttbl.ProductID into Products
                                     from product in Products.DefaultIfEmpty()
                                 
                                     where shoe.Price >= rangceitem.min
                                     select new
                                     {
                                         shoe.ShoesDetailsId,
                                         shoe.ShoesDetailsCode,
                                         shoe.DateCreated,
                                         shoe.Price,
                                         shoe.ImportPrice,
                                         shoe.ProductID,
                                         shoe.Description,
                                         shoe.Status,
                                         shoe.ColorID,
                                         shoe.StyleID,
                                         product.SupplierID,
                                         shoe.ImageUrl,
                                         product.Name,
                                         shoe.SexID,
                                     });

                        foreach(var item in query.ToList())
                        {
                            shoesDetailsDTO shoesDetails = new shoesDetailsDTO();
                            shoesDetails.ShoesDetailsId = item.ShoesDetailsId;
                            shoesDetails.ShoesDetailsCode = item.ShoesDetailsCode;
                            shoesDetails.DateCreated = item.DateCreated;
                            shoesDetails.Price = item.Price;
                            shoesDetails.ImportPrice = item.ImportPrice;
                            shoesDetails.ProductID = item.ProductID;
                            shoesDetails.Description = item.Description;
                            shoesDetails.Status = item.Status;
                            shoesDetails.ColorID = item.ColorID;
                            shoesDetails.StyleID = item.StyleID;
                            shoesDetails.ImageUrl = item.ImageUrl;
                            shoesDetails.SexID = item.SexID;
                            shoesDetails.Name = item.Name;
                            shoesDetails.SupplierID = item.SupplierID;

                            respro.Shoe_Details.Add(shoesDetails);  
                        }

                    } else
                    { 
                        var query = (from shoe in shoeList

                                     join producttbl in _dbContext.Products.AsQueryable()
                                     on shoe.ProductID equals producttbl.ProductID into Products
                                     from product in Products.DefaultIfEmpty()

                                     where shoe.Price > rangceitem.min && shoe.Price <= rangceitem.max
                                     select new shoesDetailsDTO()
                                        {
                                          ShoesDetailsId = shoe.ShoesDetailsId,
                                          ShoesDetailsCode =  shoe.ShoesDetailsCode,
                                          DateCreated =  shoe.DateCreated,
                                          Price =  shoe.Price,
                                           ImportPrice = shoe.ImportPrice,
                                           ProductID = shoe.ProductID,
                                            Description = shoe.Description,
                                           Status = shoe.Status,
                                            ColorID = shoe.ColorID,
                                            StyleID = shoe.StyleID,
                                            ImageUrl = shoe.ImageUrl,
                                            SupplierID = product.SupplierID,
                                            Name =  product.Name,
                                            SexID = shoe.SexID,
                                        });




                        foreach (var item in query.ToList())
                        {
                            shoesDetailsDTO shoesDetails = new shoesDetailsDTO()
                            {
                                ShoesDetailsId = item.ShoesDetailsId,
                                ShoesDetailsCode = item.ShoesDetailsCode,
                                DateCreated = item.DateCreated,
                                Price = item.Price,
                                ImportPrice = item.ImportPrice,
                                ProductID = item.ProductID,
                                Description = item.Description,
                                Status = item.Status,
                                ColorID = item.ColorID,
                                StyleID = item.StyleID,
                                ImageUrl = item.ImageUrl,
                                SexID = item.SexID,
                                Name = item.Name,
                                SupplierID = item.SupplierID,
                            };
                            respro.Shoe_Details.Add(shoesDetails);
                        }

                    }
                }
                respro.status = 200;
                respro.Message = "Get Products Successfully!";
                return respro;
            }
            catch (Exception ex)
            {
                respro.Message = ex.Message;
                respro.status = 400;
                return respro;
            }
        }
    }
}
