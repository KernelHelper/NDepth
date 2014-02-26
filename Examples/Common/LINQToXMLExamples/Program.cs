namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void Main()
        {
            // Base LINQ to XML types.
            CreateXElements();
            CloneXElements();
            CreateXAttributes();
            CreateXDocument();
            AttachVsClone();

            // Parsing XML.
            ParseXmlString();
            ParseXmlStringWithError();
            LoadXmlFile();
            LoadXmlFileWithOptions();
            LoadXmlReader();
            LoadXmlStreamingReader();

            // XSLT transformations.
            TransformWithXslt();

            // DTD/XSD validations.
            ValidateWithDtd();
            ValidateWithXsd();

            // XML content management.
            ModifyXmlWithAdd();
            ModifyXmlWithAddFirst();
            ModifyXmlWithAddAfterSelf();
            ModifyXmlWithAddBeforeSelf();
            ModifyXmlWithReplaceAll();
            ModifyXmlWithReplaceAttributes();
            ModifyXmlWithReplaceNodes();
            ModifyXmlWithReplaceWith();
            ModifyXmlWithRemove();
            ModifyXmlWithRemoveAll();
            ModifyXmlWithRemoveAttributes();
            ModifyXmlWithElementSetValue();
            ModifyXmlWithAttributeSetValue();
            ModifyXmlWithSetElementValue();
            ModifyXmlWithSetAttributeValue();
            ModifyXmlTransformVsFunctional();

            // XML namespaces.
            CreateDefaultNamespace();
            CreateNamespacePrefix();
            CreateTwoNamespacesWithDefault();
            CreateTwoNamespacesWithoutDefault();
            CreateNamespaceExpanded();
            QueryNamespace1();
            QueryNamespace2();
            QueryNamespace3();
            QueryNamespace4();
            ChangeNamespace();

            // XML serialization.
            SerializeWithSaveOptions();
            SerializeWithXmlDeclaration();
            DeserializeWithXmlReader();

            // XML axes methods.
            XContainerElement();
            XContainerElements1();
            XContainerElements2();
            XNodeElementsAfterSelf1();
            XNodeElementsAfterSelf2();
            XNodeElementsBeforeSelf1();
            XNodeElementsBeforeSelf2();
            XNodeAncestors1();
            XNodeAncestors2();
            XElementAncestorsAndSelf1();
            XElementAncestorsAndSelf2();
            XContainerDescendants1();
            XContainerDescendants2();
            XElementDescendantsAndSelf1();
            XElementDescendantsAndSelf2();
            XElementAttributes1();
            XElementAttributes2();
            XElementAttribute();

            // XML axes operations.
            GetCollectionOfElements();
            GetElementValue();
            GetElementValueWithCheck();
            GetDescendantsFilered();
            GetElementsChain();
            GetFirstChildElement();
            GetAttributesCollection();
            GetAttributeValue();

            // Shallow value.
            GetShallowValue();

            // Query operations.
            QueryBasedOnContext();
            QueryXPath();
            QueryXPathExpression();

            // XPath operations.
            QueryXPathChildElement();
            QueryXPathChildElements();
            QueryXPathRootElement();
            QueryXPathDescendants();
            QueryXPathAttributeFilter();
            QueryXPathRelatedElements();
            QueryXPathNamespaces();
            QueryXPathFollowingSibling();
            QueryXPathPrecedingSibling();
            QueryXPathPrecedingSiblingEx();
            QueryXPathParent();
            QueryXPathParentAttribute();
            QueryXPathSpecificAttribute();
            QueryXPathPosition();
            QueryXPathText();
            QueryXPathUnion();

            // Annotations.
            Annotations();

            // Events.
            Events();
        }
    }
}
