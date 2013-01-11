namespace TCResourceVerifier.Tests
{
    // ReSharper disable InconsistentNaming

    public static class TokenTestCases
    {
        public const string Tokens_0 = @"Lorem $core_v2_language ipsum dolor sit amet, consectetur 
adipiscing elit. Suspendisse lectus sem, accumsan 
ac cursus vitae,  $core_v1_language.GetResource(""interdum ac magna""). Donec 
turpis dolor, ornare quis dignissim non, mattis eget est. Sed 
non risus quis dolor cursus luctus.GetResource 
Donec tristique, $core_v2_language.GetResource(orci sit amet commodo consequat), 
diam ligula tristique arcu, a ultricies massa libero ac dolor. 
Aenean metus dolor, core_v2_language.GetResource(blandit) at semper ac, 
feugiat quis mauris. Nulla core_v2_language.GetResource(\'bibendum\') leo 
scelerisque urna lobortis sagittis. Duis egestas varius lorem, sit amet 
lacinia justo iaculis non.";

        public const string Tokens_1 = 
            @"#set ($blog = $core_v2_blog.Current)
#if ($blog && $blog.EnableAbout)
	<h3 class=""title"">$blog.AboutTitle</h3>
	<h4 class=""description user-defined-markup"">$blog.AboutDescription</h4>
#else
	$core_v2_widget.Hide()
#end
$core_v2_language.GetResource('Token_1')";

        public const string Tokens_2 =
            @"#set ($blog = $core_v2_blog.Current)
#if ($blog && $blog.EnableAbout)
	<h3 class=""title"">$blog.AboutTitle $core_v2_language.GetResource('Token_2')</h3>
	<h4 class=""description user-defined-markup"">$blog.AboutDescription</h4>
#else
	$core_v2_widget.Hide()
#end
$core_v2_language.GetResource('Token_1')";

        public const string Tokens_3 =
            @"#set ($blog = $core_v2_blog.Current)
#if ($blog && $blog.EnableAbout)
	<h3 class=""title"">$blog.AboutTitle $core_v2_language.GetResource('Token_2')</h3>
	<h4 class=""description user-defined-markup"">$core_v2_language.GetResource(""Token_3"")$blog.AboutDescription</h4>
#else
	$core_v2_widget.Hide()
#end
$core_v2_language.GetResource('Token_1')";

        public const string Tokens_33 =
            @"#set ($blog = $core_v2_blog.Current)
#if ($blog && $blog.EnableAbout)
	<h3 class=""title"">$core_v2_language.GetResource('Token_3') $blog.AboutTitle $core_v2_language.GetResource('Token_2')</h3>
	<h4 class=""description user-defined-markup"">$core_v2_language.GetResource(""Token_3"")$blog.AboutDescription</h4>
#else
	$core_v2_widget.Hide()
#end
$core_v2_language.GetResource('Token_1')";

        public const string CurlyTokens_0 = @"
<![CDATA[
<propertyGroup id=""options"">
	<property id=""fragmentHeader"" dataType=""string"" defaultValue=""$resource:Blogs_AboutBlog_Name"" controlType=""Telligent.Evolution.Controls.ContentFragmentTokenStringControl, Telligent.Evolution.Controls"" />
</propertyGroup>";

        public const string CurlyTokens_1 = @"
<![CDATA[
<propertyGroup id=""options"">
	<property id=""fragmentHeader"" dataType=""string"" defaultValue=""${resource:Blogs_AboutBlog_Name1}"" controlType=""Telligent.Evolution.Controls.ContentFragmentTokenStringControl, Telligent.Evolution.Controls"" />
</propertyGroup>";

        public const string CurlyTokens_2 = @"
<![CDATA[
<propertyGroup id=""options"">
	<property id=""fragmentHeader"" dataType=""${resource:Blogs_AboutBlog_Name1}"" defaultValue=""${resource:Blogs_AboutBlog_Name2}"" controlType=""Telligent.Evolution.Controls.ContentFragmentTokenStringControl, Telligent.Evolution.Controls"" />
</propertyGroup>";


/*
         public const string CurlyTokens_0 = @"
<![CDATA[
<propertyGroup id=""options"" resourceName=""Options"">
	<property id=""fragmentHeader"" resourceName=""CF_Title"" dataType=""string"" defaultValue=""$resource:Blogs_AboutBlog_Name"" controlType=""Telligent.Evolution.Controls.ContentFragmentTokenStringControl, Telligent.Evolution.Controls"" />
</propertyGroup>";

        public const string CurlyTokens_1 = @"
<![CDATA[
<propertyGroup id=""options"" resourceName=""Options"">
	<property id=""fragmentHeader"" resourceName=""CF_Title"" dataType=""string"" defaultValue=""${resource:Blogs_AboutBlog_Name1}"" controlType=""Telligent.Evolution.Controls.ContentFragmentTokenStringControl, Telligent.Evolution.Controls"" />
</propertyGroup>";

        public const string CurlyTokens_2 = @"
<![CDATA[
<propertyGroup id=""options"" resourceName=""Options"">
	<property id=""fragmentHeader"" resourceName=""CF_Title"" dataType=""${resource:Blogs_AboutBlog_Name1}"" defaultValue=""${resource:Blogs_AboutBlog_Name2}"" controlType=""Telligent.Evolution.Controls.ContentFragmentTokenStringControl, Telligent.Evolution.Controls"" />
</propertyGroup>";
 */
    }
    // ReSharper restore InconsistentNaming
}
