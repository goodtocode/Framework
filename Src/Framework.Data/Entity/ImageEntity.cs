using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using GoodToCode.Extensions;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Entity
{
    /// <summary>
    /// Base DAO class for tables containing image column
    /// Separates the heavy image column into its own object
    /// </summary>
    public abstract class ImageEntity<TEntity> : EntityBase<TEntity>, IBytesKey where TEntity : ImageEntity<TEntity>, new()
    {
        private byte[] bytesValue = null;

        /// <summary>
        /// 1x1px transparent image
        /// </summary>
        public static Image Empty { get; set; } = new Bitmap(0, 0);

        /// <summary>
        /// Byte array with data
        /// </summary>
        public virtual byte[] Bytes
        {
            get
            {
                if (bytesValue == null)
                {
                    bytesValue = ImageEntity<TEntity>.Empty.ToBytes();
                }
                return bytesValue;
            }
            set
            {
                bytesValue = value;
            }
        }

        /// <summary>
        /// Image of the Bytes data
        /// </summary>
        public Image Image
        {
            get { return Bytes.ToImage(); }
        }

        /// <summary>
        /// Gets dynamic content type of the Bytes data
        /// </summary>
        public string ContentType
        {
            get
            {
                return Image.RawFormat.ToContentType();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageEntity()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imageBytes">Data to save</param>
        public ImageEntity(byte[] imageBytes)
            : this()
        {
            bytesValue = imageBytes;
        }

        /// <summary>
        /// Sizes based on height only, will use multiplier to calculate proper width
        /// </summary>
        /// <param name="newHeight">New height (width is auto)</param>

        public TEntity ToSize(int newHeight)
        {
            var returnValue = new TEntity();
            decimal multiplier = 0m;

            multiplier = (newHeight.ToDecimal() / this.Image.Height.ToDecimal());
            returnValue = this.ToSize(new Size((this.Image.Width * multiplier).ToInt(), newHeight));

            return returnValue;
        }

        /// <summary>
        /// Resizes an image and stretches if ratio is wrong
        /// </summary>
        /// <param name="newSize">New height and width</param>
        public TEntity ToSize(Size newSize)
        {
            var returnValue = new TEntity();

            if ((this.Image == null == false) && (this.Image.Size.Width > 0 & this.Image.Size.Height > 0))
            {
                returnValue.Bytes = this.Image.Resize(newSize).ToBytes();
            }

            return returnValue;
        }

        /// <summary>
        /// Converts to a lightweight thumbnail
        /// </summary>
        public TEntity ToThumbnail()
        {
            var returnValue = new TEntity();
            Image newImage = this.Image;
            Image thumbnail = newImage.GetThumbnailImage(this.Image.Width, this.Image.Height, new Image.GetThumbnailImageAbort(EmptyCallBack), IntPtr.Zero);
            returnValue.Bytes = thumbnail.ToBytes();

            return returnValue;
        }

        /// <summary>
        /// Puts a thumbnail in upper left corner
        /// </summary>
        /// <param name="width">Width in px</param>
        /// <param name="height">Height in px</param>
        public TEntity ToThumbnailInUpperLeftCorner(int width, int height)
        {
            var returnValue = new TEntity();
            var thumbnail = new Bitmap(this.Image, this.Image.Width, this.Image.Height);
            var graphicConvert = Graphics.FromImage(thumbnail);
            var rectangleConvert = new Rectangle(0, 0, width, height);

            if ((this.Image == null == false) && (this.Image.Size.Width > 0 & this.Image.Size.Height > 0))
            {
                // Resize image
                graphicConvert.CompositingQuality = CompositingQuality.HighQuality;
                graphicConvert.SmoothingMode = SmoothingMode.HighQuality;
                graphicConvert.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicConvert.DrawImage(this.Image, rectangleConvert);
                returnValue.Bytes = thumbnail.ToBytes();
            }

            return returnValue;
        }

        /// <summary>
        /// Required for ToThumbnail
        /// </summary>
        private bool EmptyCallBack()
        {
            return false;
        }

        /// <summary>
        /// Crops an x,y for the area of width, height
        /// </summary>
        /// <param name="width">Width in px</param>
        /// <param name="height">Height in px</param>
        /// <param name="x">Starting X</param>
        /// <param name="y">Starting Y</param>
        public Byte[] Crop(int width, int height, int x, int y)
        {
            return Image.Crop(width, height, x, y).ToBytes();
        }
    }
}
