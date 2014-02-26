<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <p>
  <xsl:for-each select="catalog/cd">
    <xsl:number value="position()" format="1" /> - 
    <xsl:value-of select="title" /><br />
  </xsl:for-each>
  </p>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet>