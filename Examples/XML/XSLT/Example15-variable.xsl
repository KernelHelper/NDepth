<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:variable name="header">
  <tr>
  <th>Artist</th>
  <th>Title</th>
  </tr>
</xsl:variable>

<xsl:template match="/">
  <html>
  <body>
  <table>
    <xsl:copy-of select="$header" />
    <xsl:for-each select="catalog/cd">
    <tr>
      <td><xsl:value-of select="artist"/></td>
      <td><xsl:value-of select="title"/></td>
    </tr>
    </xsl:for-each>
  </table>
  <br />
  <table>
    <xsl:copy-of select="$header" />
    <xsl:for-each select="catalog/cd">
    <tr>
      <td><xsl:value-of select="artist"/></td>
      <td><xsl:value-of select="title"/></td>
    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet>