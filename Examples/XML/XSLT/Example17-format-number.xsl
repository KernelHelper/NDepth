<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<!--

0 (Digit)
# (Digit, zero shows as absent)
. (The position of the decimal point Example: ###.##)
, (The group separator for thousands. Example: ###,###.##)
% (Displays the number as a percentage. Example: ##%)
; (Pattern separator. The first pattern will be used for positive numbers and the second for negative numbers)

-->

<xsl:template match="/">
<html>
<body>
<xsl:value-of select='format-number(500100, "#")' />
<br />
<xsl:value-of select='format-number(500100, "0")' />
<br />
<xsl:value-of select='format-number(500100, "#.00")' />
<br />
<xsl:value-of select='format-number(500100, "#.0")' />
<br />
<xsl:value-of select='format-number(500100, "###,###.00")' />
<br />
<xsl:value-of select='format-number(0.23456, "#%")' />
</body>
</html>
</xsl:template>

</xsl:stylesheet>