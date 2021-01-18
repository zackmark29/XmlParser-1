﻿namespace XmlParser
{
    public static class Samples
    {
      public static string Empty = @"<svg xmlns='http://www.w3.org/2000/svg'></svg>";

      public static string struct_svg_03_f = @"
<svg id=""svg-root"" width=""100%"" height=""100%""
  viewBox=""0 0 480 360"" xmlns=""http://www.w3.org/2000/svg""
  xmlns:xlink=""http://www.w3.org/1999/xlink"">
  <!--======================================================================-->
  <!--=  SVG 1.1 2nd Edition Test Case                                     =-->
  <!--======================================================================-->
  <!--=  Copyright 2008 World Wide Web Consortium, (Massachusetts          =-->
  <!--=  Institute of Technology, European Research Consortium for         =-->
  <!--=  Informatics and Mathematics (ERCIM), Keio University).            =-->
  <!--=  All Rights Reserved.                                              =-->
  <!--=  See http://www.w3.org/Consortium/Legal/.                          =-->
  <!--======================================================================-->
  <d:SVGTestCase xmlns:d=""http://www.w3.org/2000/02/svg/testsuite/description/""
    template-version=""1.4"" reviewer=""[reviewer]"" author=""ED"" status=""created""
    version=""$Revision: 1.1 $"" testname=""$RCSfile: struct-svg-03-f.svg,v $"">
    <d:testDescription xmlns=""http://www.w3.org/1999/xhtml"" href=""http://www.w3.org/TR/SVG11/struct.html#ImageElement"">
      <p>
        Test nested svg elements.
      </p>
    </d:testDescription>
    <d:operatorScript xmlns=""http://www.w3.org/1999/xhtml"">
      <p>
        Run the test. No interaction required.
      </p>
    </d:operatorScript>
    <d:passCriteria xmlns=""http://www.w3.org/1999/xhtml"">
      <p>
        Passed if there are two green rectangles visible, and no red.
      </p>
    </d:passCriteria>
  </d:SVGTestCase>
  <title id=""test-title"">$RCSfile: struct-svg-03-f.svg,v $</title>
  <defs>
    <font-face
      font-family=""SVGFreeSansASCII""
      unicode-range=""U+0-7F"">
      <font-face-src>
        <font-face-uri xlink:href=""../resources/SVGFreeSans.svg#ascii""/>
      </font-face-src>
    </font-face>
  </defs>
  <g id=""test-body-content"" font-family=""SVGFreeSansASCII,sans-serif"" font-size=""18"">

    <rect x=""0.5"" y=""0.5"" width=""99"" height=""99"" fill=""red""/>
    <svg width=""100"" height=""100"" viewBox=""0 0 50 50"">
      <rect width=""50"" height=""50"" fill=""lime""/>
    </svg>
    
    <g transform=""translate(120 0)"">
      <rect x=""0.5"" y=""0.5"" width=""99"" height=""99"" fill=""red""/>
      <svg width=""100"" height=""100"" viewBox=""0 0 50 50"">
        <svg width=""50"" height=""50"" viewBox=""0 0 200 200"">
          <rect width=""200"" height=""200"" fill=""lime""/>
        </svg>
      </svg>
    </g>
  </g>
  <g font-family=""SVGFreeSansASCII,sans-serif"" font-size=""32"">
    <text id=""revision"" x=""10"" y=""340"" stroke=""none""
      fill=""black"">$Revision: 1.1 $</text>
  </g>
  <rect id=""test-frame"" x=""1"" y=""1"" width=""478"" height=""358"" fill=""none"" stroke=""#000""/>
  <g id=""draft-watermark"">
    <rect x=""1"" y=""1"" width=""478"" height=""20"" fill=""red"" stroke=""black"" stroke-width=""1""/>
    <text font-family=""SVGFreeSansASCII,sans-serif"" font-weight=""bold"" font-size=""20"" x=""240""
      text-anchor=""middle"" y=""18"" stroke-width=""0.5"" stroke=""black"" fill=""white"">DRAFT</text>
  </g>
</svg>
";
    }
}
