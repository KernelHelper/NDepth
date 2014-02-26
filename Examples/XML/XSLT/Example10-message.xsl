<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <xsl:for-each select="catalog/cd">
    <p>Title: <xsl:value-of select="title"/><br />
    Artist:
    <xsl:if test="artist=''">
      <xsl:message terminate="yes">
        Error: Artist is an empty string!
      </xsl:message>
    </xsl:if>
    <xsl:value-of select="artist"/>
    </p>
  </xsl:for-each>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet>