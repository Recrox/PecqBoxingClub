using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.RamDamApi
{

    public class RamDamPlace
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_gmt { get; set; }
        public Guid Guid { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Modified_gmt { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public Title Title { get; set; }
        public Content Content { get; set; }
        public int Featured_media { get; set; }
        public string Template { get; set; }
        public Acf Acf { get; set; }
        public _Links _links { get; set; }
    }

    public class Guid
    {
        public string Rendered { get; set; }
    }

    public class Title
    {
        public string Rendered { get; set; }
    }

    public class Content
    {
        public string Rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Acf
    {
        [JsonProperty("address-type")]
        public string Addresstype { get; set; }
        public object Place { get; set; }
        public string Contact { get; set; }
    }

    public class Place
    {
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }

    public class _Links
    {
        public Self[] Self { get; set; }
        public Collection[] Collection { get; set; }
        public About[] About { get; set; }
        public WpAttachment[] Wpattachment { get; set; }
        public Cury[] Curies { get; set; }
        public WpFeaturedmedia[] Wpfeaturedmedia { get; set; }
    }

    public class Self
    {
        public string Href { get; set; }
    }

    public class Collection
    {
        public string Href { get; set; }
    }

    public class About
    {
        public string Href { get; set; }
    }

    public class WpAttachment
    {
        public string Href { get; set; }
    }

    public class Cury
    {
        public string Name { get; set; }
        public string Href { get; set; }
        public bool Templated { get; set; }
    }

    public class WpFeaturedmedia
    {
        public bool Embeddable { get; set; }
        public string Href { get; set; }
    }

}
