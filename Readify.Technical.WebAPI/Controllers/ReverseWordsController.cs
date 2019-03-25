using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Readify.Technical.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReverseWordsController : Controller
    {

        [HttpGet]
        
        public IActionResult Get([FromQuery]string sentence)
        {
            try { 
            return Ok(GetReverseWords(sentence));
            } catch(Exception ex) { return BadRequest(); }
        }

        private string GetReverseWords(string sentence)
        {
            if (sentence == null)
                return string.Empty;
            StringBuilder reverseSentence = new StringBuilder();
            string[] words = sentence.Split(" ");
            bool endsWithSpace = sentence.EndsWith(" ");
            for(int i=0;i<words.Length;i++)// (string word in words)
            {
                var tmp = words[i].ToCharArray();
                Array.Reverse(tmp);
                reverseSentence.Append(new string(tmp));
                if (i+1!=words.Length)
                reverseSentence.Append(" ");
                 
            }
            return  (endsWithSpace) ? reverseSentence.Append(" ").ToString() :  reverseSentence.ToString();
        }
    }
}
