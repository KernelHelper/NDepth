<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <xsl:call-template name="show_title">
    <xsl:with-param name="param" select="title"/>
  </xsl:call-template>
  </body>
  </html>
</xsl:template>

<xsl:template name="show_title">
  <xsl:param name="param"/>
  <xsl:for-each select="catalog/cd">
    <p>Title: <xsl:value-of select="$param"/></p>
  </xsl:for-each>
</xsl:template>

</xsl:stylesheet>