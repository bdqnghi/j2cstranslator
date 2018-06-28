package test;

import java.io.File;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;

import junit.framework.Assert;
import junit.framework.TestCase;
import junit.framework.TestSuite;

import org.eclipse.core.resources.IWorkspace;
import org.junit.Before;
import org.junit.Test;

import com.ilog.translator.java2cs.plugin.TranslationPlugin;

public class IntegrationTests {

	// private static IWorkspace workspace = TranslationPlugin.getWorkspace();
	// IWorkspaceDescription wsDesc = null;
	private static String pathToTests = "C:/work/Repositories/Translator/EclipsePlugin/testWorkspace/";

	/*
	 * @Before public void setUp() throws Exception { workspace =
	 * TranslationPlugin.getWorkspace(); wsDesc = workspace.getDescription(); }
	 */

	//
	//
	//
	/*
	 * @Test public void affectChar() { translationTest("affectChar"); }
	 * 
	 * @Test public void Annotation() { translationTest("Annotation"); }
	 * 
	 * @Test public void Anonymous() { translationTest("Anonymous"); }
	 * 
	 * @Test public void AnonymousClassWithFieldTests() {
	 * translationTest("AnonymousClassWithFieldTests"); }
	 * 
	 * @Test public void AnonymousInInterfaceField() {
	 * translationTest("AnonymousInInterfaceField"); }
	 * 
	 * @Test public void AnonymousInMethod() {
	 * translationTest("AnonymousInMethod"); }
	 * 
	 * @Test public void AnonymousSubClass() {
	 * translationTest("AnonymousSubClass"); }
	 * 
	 * @Test public void ArrasTests() { translationTest("ArrasTests"); }
	 * 
	 * @Test public void ArrayDotClass() { translationTest("ArrayDotClass"); }
	 * 
	 * @Test public void ArrayListTest() { translationTest("ArrayListTest"); }
	 * 
	 * @Test public void AsListTest() { translationTest("AsListTest"); }
	 * 
	 * @Test public void Autoboxing() { translationTest("Autoboxing"); }
	 * 
	 * @Test public void AutomaticInferMethod() {
	 * translationTest("AutomaticInferMethod"); }
	 * 
	 * @Test public void AutoProperty() { translationTest("AutoProperty"); }
	 * 
	 * @Test public void AutoProperty2() { translationTest("AutoProperty2"); }
	 * 
	 * @Test public void AutoProperty3() { translationTest("AutoProperty3"); }
	 * 
	 * @Test public void AutoProperty3Dept() {
	 * translationTest("AutoProperty3Dept"); }
	 * 
	 * @Test public void AutoProperty4() { translationTest("AutoProperty4"); }
	 * 
	 * @Test public void AutoPropertyWithForeach() {
	 * translationTest("AutoPropertyWithForeach"); }
	 * 
	 * @Test public void BitSetJdk6() { translationTest("BitSetJdk6"); }
	 * 
	 * @Test public void BooleanTests() { translationTest("BooleanTests"); }
	 * 
	 * @Test public void BooleanTrue() { translationTest("BooleanTrue"); }
	 * 
	 * @Test public void BreakInSwitch() { translationTest("BreakInSwitch"); }
	 * 
	 * @Test public void BugNameWithSpace() { translationTest("Bug Name With
	 * Space"); }
	 * 
	 * @Test public void BugCapitalizeMethod() {
	 * translationTest("BugCapitalizeMethod"); }
	 * 
	 * @Test public void BugCapitalizeMethodDep() {
	 * translationTest("BugCapitalizeMethodDep"); }
	 * 
	 * @Test public void CapitalizationTest() {
	 * translationTest("CapitalizationTest"); }
	 * 
	 * @Test public void ChainOfCall() { translationTest("ChainOfCall"); }
	 * 
	 * @Test public void CharAndIntTests() { translationTest("CharAndIntTests"); }
	 * 
	 * @Test public void ClassFilterTests() {
	 * translationTest("ClassFilterTests"); }
	 * 
	 * @Test public void ClassInMethodTest() {
	 * translationTest("ClassInMethodTest"); }
	 * 
	 * @Test public void ClassOfT() { translationTest("ClassOfT"); }
	 * 
	 * @Test public void ClassTests() { translationTest("ClassTests"); }
	 * 
	 * @Test public void Collections() { translationTest("Collections"); }
	 * 
	 * @Test public void CommentsTests() { translationTest("CommentsTests"); }
	 * 
	 * @Test public void ComputeArraySignatureBugs() {
	 * translationTest("ComputeArraySignatureBugs"); }
	 * 
	 * @Test public void ConditionalExpressionGenericTest() {
	 * translationTest("ConditionalExpressionGenericTest"); }
	 * 
	 * @Test public void ConditionalExpressionTest() {
	 * translationTest("ConditionalExpressionTest"); }
	 * 
	 * @Test public void ConfParserError() { translationTest("ConfParserError"); }
	 * 
	 * @Test public void ConstantsInInterfaceBug() {
	 * translationTest("ConstantsInInterfaceBug"); }
	 * 
	 * @Test public void ConstantsInInterfaceTests() {
	 * translationTest("ConstantsInInterfaceTests"); }
	 * 
	 * @Test public void Covariance() { translationTest("Covariance"); }
	 * 
	 * @Test public void Covariance2() { translationTest("Covariance2"); }
	 * 
	 * @Test public void Covariance3() { translationTest("Covariance3"); }
	 * 
	 * @Test public void Covariance4() { translationTest("Covariance4"); }
	 * 
	 * @Test public void DefaultMembersMappingTest() {
	 * translationTest("DefaultMembersMappingTest"); }
	 * 
	 * @Test public void DefaultOfT() { translationTest("DefaultOfT"); }
	 * 
	 * @Test public void DefaultPackageMappingTest() {
	 * translationTest("DefaultPackageMappingTest"); }
	 * 
	 * @Test public void DotClass() { translationTest("DotClass"); }
	 * 
	 * @Test public void DotClassGeneric() { translationTest("DotClassGeneric"); }
	 * 
	 * @Test public void EditBrowsableFalse() {
	 * translationTest("EditBrowsableFalse"); }
	 * 
	 * @Test public void EditBrowsableTrue() {
	 * translationTest("EditBrowsableTrue"); }
	 * 
	 * @Test public void Enum() { translationTest("Enum"); }
	 * 
	 * @Test public void Enum2() { translationTest("Enum2"); }
	 * 
	 * @Test public void EnumMap() { translationTest("EnumMap"); }
	 * 
	 * @Test public void EnumSet() { translationTest("EnumSet"); }
	 * 
	 * @Test public void ExtraSemicolonTest() {
	 * translationTest("ExtraSemicolonTest"); }
	 * 
	 * @Test public void FalseNestedAccess() {
	 * translationTest("FalseNestedAccess"); }
	 * 
	 * @Test public void FieldAccesAutoRename() {
	 * translationTest("FieldAccesAutoRename"); }
	 * 
	 * @Test public void FieldInFieldReplacementName() {
	 * translationTest("FieldInFieldReplacementName"); }
	 * 
	 * @Test public void FieldsInitInInterface() {
	 * translationTest("FieldsInitInInterface"); }
	 * 
	 * @Test public void FielsInitTests() { translationTest("FielsInitTests"); }
	 * 
	 * @Test public void FinalFieldsMove() { translationTest("FinalFieldsMove"); }
	 * 
	 * @Test public void FinalMemberVisibility() {
	 * translationTest("FinalMemberVisibility"); }
	 * 
	 * @Test public void FinalMemberVisibilitySimple() {
	 * translationTest("FinalMemberVisibilitySimple"); }
	 * 
	 * @Test public void FinalTests() { translationTest("FinalTests"); }
	 * 
	 * @Test public void FlatPatternTest() { translationTest("FlatPatternTest"); }
	 * 
	 * @Test public void FloatLiteral() { translationTest("FloatLiteral"); }
	 * 
	 * @Test public void Foreach() { translationTest("Foreach"); }
	 * 
	 * @Test public void FullQualifiedStaticMember() {
	 * translationTest("FullQualifiedStaticMember"); }
	 * 
	 * @Test public void FullyGenericsOnMethodReturnTypeTest() {
	 * translationTest("FullyGenericsOnMethodReturnTypeTest"); }
	 * 
	 * @Test public void FullyGenericsTest() {
	 * translationTest("FullyGenericsTest"); }
	 * 
	 * @Test public void GenericClassOfT() { translationTest("GenericClassOfT"); }
	 * 
	 * @Test public void GenericImplements() {
	 * translationTest("GenericImplements"); }
	 * 
	 * @Test public void GenericInterface() {
	 * translationTest("GenericInterface"); }
	 * 
	 * @Test public void GenericMethod() { translationTest("GenericMethod"); }
	 * 
	 * @Test public void GenericMethod2() { translationTest("GenericMethod2"); }
	 * 
	 * @Test public void GenericMethod3() { translationTest("GenericMethod3"); }
	 * 
	 * @Test public void GenericMethod4() { translationTest("GenericMethod4"); }
	 * 
	 * @Test public void GenericMethodWithConstraint() {
	 * translationTest("GenericMethodWithConstraint"); }
	 * 
	 * @Test public void GenericMethodWithGenericConstraint() {
	 * translationTest("GenericMethodWithGenericConstraint"); }
	 * 
	 * @Test public void GenericReturnType() {
	 * translationTest("GenericReturnType"); }
	 * 
	 * @Test public void GenericsColections() {
	 * translationTest("GenericsColections"); }
	 * 
	 * @Test public void GenericsCollections2() {
	 * translationTest("GenericsCollections2"); }
	 * 
	 * @Test public void GenericsInConf() { translationTest("GenericsInConf"); }
	 * 
	 * @Test public void GenericsInner() { translationTest("GenericsInner"); }
	 * 
	 * @Test public void GenericsParameterInNestedGeneric() {
	 * translationTest("GenericsParameterInNestedGeneric"); }
	 * 
	 * @Test public void GenericsParameterInNestedGeneric2() {
	 * translationTest("GenericsParameterInNestedGeneric2"); }
	 * 
	 * @Test public void GenericsParameters() {
	 * translationTest("GenericsParameters"); }
	 * 
	 * @Test public void GenericsQM() { translationTest("GenericsQM"); }
	 * 
	 * @Test public void HashMapTests() { translationTest("HashMapTests"); }
	 * 
	 * @Test public void HashtableGet() { translationTest("HashtableGet"); }
	 * 
	 * @Test public void HashtableTests() { translationTest("HashtableTests"); }
	 * 
	 * @Test public void HashtableTestsDep() {
	 * translationTest("HashtableTestsDep"); }
	 * 
	 * @Test public void ImplicitFieldRename() {
	 * translationTest("ImplicitFieldRename"); }
	 * 
	 * @Test public void InitializerTest() { translationTest("InitializerTest"); }
	 * 
	 * @Test public void InitOfArray() { translationTest("InitOfArray"); } /*
	 * @Test public void Inner3Level() { translationTest("Inner3Level"); }
	 * 
	 * 
	 * @Test public void InnerAccess() { translationTest("InnerAccess"); }
	 * 
	 * @Test public void InnerAccess2() { translationTest("InnerAccess2"); }
	 * 
	 * @Test public void InnerAccess3() { translationTest("InnerAccess3"); }
	 * 
	 * @Test public void InnerAccess4() { translationTest("InnerAccess4"); }
	 * 
	 * @Test public void InnerGeneric() { translationTest("InnerGeneric"); }
	 * 
	 * @Test public void InnerSuperTests() { translationTest("InnerSuperTests"); }
	 * 
	 * @Test public void InstanceOf() { translationTest("InstanceOf"); }
	 * 
	 * @Test public void InstanceOfGenerics() {
	 * translationTest("InstanceOfGenerics"); }
	 * 
	 * @Test public void InterfaceWithFieldInit() {
	 * translationTest("InterfaceWithFieldInit"); }
	 * 
	 * @Test public void InterfaceWithFieldInitDep() {
	 * translationTest("InterfaceWithFieldInitDep"); }
	 * 
	 * @Test public void IteratorTests() { translationTest("IteratorTests"); }
	 * 
	 * @Test public void JavaDocTest() { translationTest("JavaDocTest"); }
	 * 
	 * @Test public void JavaUtilCollections() {
	 * translationTest("JavaUtilCollections"); }
	 * 
	 * @Test public void JumboEnumSet() { translationTest("JumboEnumSet"); }
	 * 
	 * @Test public void JUnit4Test() { translationTest("JUnit4Test"); }
	 * 
	 * @Test public void LoopTests() { translationTest("LoopTests"); }
	 * 
	 * @Test public void ManualIndexer() { translationTest("ManualIndexer"); }
	 * 
	 * @Test public void ManualProperty() { translationTest("ManualProperty"); }
	 * 
	 * @Test public void ManyInitializersTests() {
	 * translationTest("ManyInitializersTests"); }
	 * 
	 * @Test public void MappingsInComments() {
	 * translationTest("MappingsInComments"); }
	 * 
	 * @Test public void MethodCallWithFormat() {
	 * translationTest("MethodCallWithFormat"); }
	 * 
	 * @Test public void MixGenericsCollections() {
	 * translationTest("MixGenericsCollections"); }
	 * 
	 * @Test public void MoveFieldBug() { translationTest("MoveFieldBug"); }
	 * 
	 * @Test public void MoveFieldBugSimple() {
	 * translationTest("MoveFieldBugSimple"); }
	 * 
	 * @Test public void MyFirstProject() { translationTest("MyFirstProject"); }
	 * 
	 * @Test public void NestedClassMemberModifiers() {
	 * translationTest("NestedClassMemberModifiers"); }
	 * 
	 * @Test public void NestedToInnerCrossProject() {
	 * translationTest("NestedToInnerCrossProject"); }
	 * 
	 * @Test public void NestedToInnerCrossProjectDep() {
	 * translationTest("NestedToInnerCrossProjectDep"); }
	 * 
	 * @Test public void NewArray() { translationTest("NewArray"); }
	 * 
	 * @Test public void NonPublicClass() { translationTest("NonPublicClass"); }
	 * 
	 * @Test public void NotJavaDocCommentsTests() {
	 * translationTest("NotJavaDocCommentsTests"); }
	 * 
	 * @Test public void ObjectEqual() { translationTest("ObjectEqual"); }
	 * 
	 * @Test public void OrderOfParameterInAnonymous() {
	 * translationTest("OrderOfParameterInAnonymous"); }
	 * 
	 * @Test public void PackageTypeConflict() {
	 * translationTest("PackageTypeConflict"); }
	 * 
	 * @Test public void PackageTypeConflictGenerics() {
	 * translationTest("PackageTypeConflictGenerics"); }
	 * 
	 * @Test public void PbIndent() { translationTest("PbIndent"); }
	 * 
	 * @Test public void PointerEquality() { translationTest("PointerEquality"); }
	 * 
	 * @Test public void PrimitiveTypes() { translationTest("PrimitiveTypes"); }
	 * 
	 * @Test public void ProxyMode() { translationTest("ProxyMode"); }
	 * 
	 * @Test public void QualifiedInnerGenerics() {
	 * translationTest("QualifiedInnerGenerics"); }
	 * 
	 * /*@Test public void ReadOnlySourcesTest() {
	 * translationTest("ReadOnlySourcesTest"); }*
	 * 
	 * @Test public void RefactorClassInfo() {
	 * translationTest("RefactorClassInfo"); }
	 * 
	 * @Test public void ReferenceToInner() {
	 * translationTest("ReferenceToInner"); }
	 * 
	 * @Test public void RemoveAutoInternal() {
	 * translationTest("RemoveAutoInternal"); }
	 * 
	 * @Test public void RemoveInnerClassTest() {
	 * translationTest("RemoveInnerClassTest"); }
	 * 
	 * @Test public void RenamedMapping() { translationTest("RenamedMapping"); }
	 * 
	 * @Test public void RenamedMappingUse() {
	 * translationTest("RenamedMappingUse"); }
	 * 
	 * @Test public void RenameFieldsTest() {
	 * translationTest("RenameFieldsTest"); }
	 * 
	 * @Test public void RenameMethod() { translationTest("RenameMethod"); }
	 * 
	 * @Test public void RenameMethodAndSubClass() {
	 * translationTest("RenameMethodAndSubClass"); }
	 * 
	 * @Test public void RenameMethodAndSubClassDep() {
	 * translationTest("RenameMethodAndSubClassDep"); }
	 * 
	 * @Test public void RenameMethodWithInnerParam() {
	 * translationTest("RenameMethodWithInnerParam"); }
	 * 
	 * @Test public void RenamePrivateFinalFields() {
	 * translationTest("RenamePrivateFinalFields"); }
	 * 
	 * @Test public void RenameValueFieldTest() {
	 * translationTest("RenameValueFieldTest"); }
	 * 
	 * @Test public void ReplaceTypeWithComments() {
	 * translationTest("ReplaceTypeWithComments"); }
	 * 
	 * @Test public void SerializableTest() {
	 * translationTest("SerializableTest"); }
	 * 
	 * @Test public void SimpleProject() { translationTest("SimpleProject"); }
	 * 
	 * @Test public void SimpleProjectsrc() {
	 * translationTest("SimpleProjectsrc"); }
	 * 
	 * @Test public void StackTest() { translationTest("StackTest"); }
	 * 
	 * @Test public void StringTests() { translationTest("StringTests"); }
	 * 
	 * @Test public void SubClass() { translationTest("SubClass"); }
	 * 
	 * @Test public void SubClassOfExceptionTest() {
	 * translationTest("SubClassOfExceptionTest"); }
	 * 
	 * @Test public void SubClassOfGeneric() {
	 * translationTest("SubClassOfGeneric"); }
	 * 
	 * @Test public void SubClassProbleme() {
	 * translationTest("SubClassProbleme"); }
	 * 
	 * @Test public void SuffixTests() { translationTest("SuffixTests"); }
	 * 
	 * @Test public void SwitchInSwitchTest() {
	 * translationTest("SwitchInSwitchTest"); }
	 * 
	 * @Test public void SwitchTest() { translationTest("SwitchTest"); }
	 * 
	 * @Test public void SystemOutPrint() { translationTest("SystemOutPrint"); }
	 * 
	 * @Test public void TestNGTest() { translationTest("TestNGTest"); }
	 * 
	 * @Test public void ThisTest() { translationTest("ThisTest"); }
	 * 
	 * @Test public void ThreadLocal() { translationTest("ThreadLocal"); }
	 * 
	 * @Test public void ToArrayTests() { translationTest("ToArrayTests"); }
	 * 
	 * @Test public void Varargs() { translationTest("Varargs"); }
	 * 
	 * @Test public void VariableVisibility() {
	 * translationTest("VariableVisibility"); }
	 * 
	 * @Test public void VirtOver() { translationTest("VirtOver"); }
	 * 
	 * @Test public void VirtOver2() { translationTest("VirtOver2"); }
	 * 
	 * @Test public void VirtOver3() { translationTest("VirtOver3"); }
	 * 
	 * @Test public void VirtOver4() { translationTest("VirtOver4"); }
	 * 
	 * @Test public void VirtOver5() { translationTest("VirtOver5"); }
	 * 
	 * @Test public void Wildcard() { translationTest("Wildcard"); }
	 * 
	 * @Test public void WildcardTests() { translationTest("WildcardTests"); }
	 * 
	 * @Test public void WildcardTests2() { translationTest("WildcardTests2"); }
	 * 
	 * @Test public void WildcardTests3() { translationTest("WildcardTests3"); } // // //
	 * 
	 * @Test public void AddCastFroCollection() {
	 * translationTest("AddCastFroCollection"); }
	 * 
	 * @Test public void AddConstraintsTest() {
	 * translationTest("AddConstraintsTest"); }
	 * 
	 * @Test public void AnnotationDeclarationTest() {
	 * translationTest("AnnotationDeclarationTest"); }
	 * 
	 * @Test public void AutomaticInferReturnTypeTest() {
	 * translationTest("AutomaticInferReturnTypeTest"); }
	 * 
	 * @Test public void BR1969480Properties() { translationTest("BR1969480
	 * Properties"); }
	 * 
	 * @Test public void BR2017556_InnerEMF() {
	 * translationTest("BR2017556_InnerEMF"); }
	 * 
	 * @Test public void BR2070172ExtendsImplements() {
	 * translationTest("BR2070172-ExtendsImplements"); }
	 * 
	 * @Test public void BR2070179ArrayOfArray() {
	 * translationTest("BR2070179-ArrayOfArray"); }
	 * 
	 * @Test public void BR2071356UncheckedKetword() {
	 * translationTest("BR2071356-UncheckedKetword"); }
	 * 
	 * @Test public void BR2073421() { translationTest("BR2073421"); }
	 * 
	 * @Test public void BR2094467() { translationTest("BR2094467"); }
	 * 
	 * @Test public void BrandNewProject() { translationTest("BrandNewProject"); }
	 * 
	 * @Test public void BreakInSwitch2() { translationTest("BreakInSwitch2"); }
	 * 
	 * @Test public void BugFQNReplacementIn34() {
	 * translationTest("BugFQNReplacementIn3.4"); }
	 * 
	 * @Test public void BugThisTest() { translationTest("BugThisTest"); }
	 * 
	 * @Test public void ByteTest() { translationTest("ByteTest"); }
	 * 
	 * @Test public void CapitalizeTests() { translationTest("CapitalizeTests"); }
	 * 
	 * @Test public void CapitalizeTests2() {
	 * translationTest("CapitalizeTests2"); }
	 * 
	 * @Test public void ChangeHierarchieTests() {
	 * translationTest("ChangeHierarchieTests"); }
	 * 
	 * @Test public void ComplexParameterTypesTest() {
	 * translationTest("ComplexParameterTypesTest"); }
	 * 
	 * @Test public void ComplexParameterTypesTest2() {
	 * translationTest("ComplexParameterTypesTest2"); }
	 * 
	 * @Test public void ComplexParameterTypesTest3() {
	 * translationTest("ComplexParameterTypesTest3"); }
	 * 
	 * @Test public void ComplexParameterTypesTest4() {
	 * translationTest("ComplexParameterTypesTest4"); }
	 * 
	 * @Test public void DotClass2() { translationTest("DotClass2"); }
	 * 
	 * @Test public void DotClass4() { translationTest("DotClass4"); }
	 * 
	 * @Test public void DotClass4Dep() { translationTest("DotClass4Dep"); }
	 * 
	 * @Test public void DotClass4Dep2() { translationTest("DotClass4Dep2"); }
	 * 
	 * @Test public void DoubleQuoteTests() {
	 * translationTest("DoubleQuoteTests"); }
	 * 
	 * @Test public void Enum3() { translationTest("Enum3"); }
	 * 
	 * @Test public void Enum4() { translationTest("Enum4"); }
	 * 
	 * @Test public void EnumFull() { translationTest("EnumFull"); }
	 * 
	 * @Test public void EnumInInterface() { translationTest("EnumInInterface"); }
	 * 
	 * @Test public void ExtraImportsTests() {
	 * translationTest("ExtraImportsTests"); }
	 * 
	 * @Test public void FinalInStaticInitializerTests() {
	 * translationTest("FinalInStaticInitializerTests"); }
	 * 
	 * @Test public void ForeachWithComments() {
	 * translationTest("ForeachWithComments"); }
	 * 
	 * @Test public void GenericMethod5() { translationTest("GenericMethod5"); }
	 * 
	 * @Test public void IntializerTest2() { translationTest("IntializerTest2"); }
	 * 
	 * @Test public void JaggedArrayTest() { translationTest("JaggedArrayTest"); }
	 * 
	 * @Test public void JavaDocMappingsTest() {
	 * translationTest("JavaDocMappingsTest"); }
	 * 
	 * @Test public void JavaDocTest2() { translationTest("JavaDocTest2"); }
	 * 
	 * @Test public void JUnitTest2() { translationTest("JUnitTest2"); }
	 * 
	 * @Test public void MapTest() { translationTest("MapTest"); }
	 * 
	 * @Test public void MavenStructureTest() {
	 * translationTest("MavenStructureTest"); }
	 * 
	 * @Test public void MyFirstProject2() { translationTest("MyFirstProject2"); }
	 * 
	 * @Test public void NestedGenericTest() {
	 * translationTest("NestedGenericTest"); }
	 * 
	 * @Test public void ParseExceptionTests() {
	 * translationTest("ParseExceptionTests"); }
	 * 
	 * @Test public void PbSAM() { translationTest("PbSAM"); }
	 * 
	 * @Test public void ProjectNameTestLonger() { translationTest("Project Name
	 * Test Longer"); }
	 * 
	 * @Test public void ProjectNameWithSpace() { translationTest("Project Name
	 * With Space"); }
	 * 
	 * @Test public void ProtectedInternalTests() {
	 * translationTest("ProtectedInternalTests"); }
	 * 
	 * @Test public void RenamedClassChangeModifier() {
	 * translationTest("RenamedClassChangeModifier"); }
	 * 
	 * @Test public void ROOTPropertyTest() {
	 * translationTest("ROOTPropertyTest"); }
	 * 
	 * @Test public void SingletonListTests() {
	 * translationTest("SingletonListTests"); }
	 * 
	 * @Test public void StringFormatTests() {
	 * translationTest("StringFormatTests"); }
	 * 
	 * @Test public void SubcListClassTest() {
	 * translationTest("SubcListClassTest"); }
	 * 
	 * @Test public void SubListTest() { translationTest("SubListTest"); }
	 * 
	 * @Test public void SuperorThisConstructorInvokTest() {
	 * translationTest("SuperorThisConstructorInvokTest"); }
	 * 
	 * @Test public void TestHelloWord() { translationTest("TestHelloWord"); }
	 * 
	 * @Test public void TestLog() { translationTest("TestLog"); }
	 * 
	 * @Test public void ThisTest2() { translationTest("ThisTest2"); }
	 * 
	 * @Test public void ThisTest3() { translationTest("ThisTest3"); }
	 * 
	 * @Test public void ThisTest4() { translationTest("ThisTest4"); }
	 * 
	 * @Test public void URSRenameTest() { translationTest("URSRenameTest"); }
	 * 
	 * @Test public void Varargs2() { translationTest("Varargs2"); }
	 */
	//
	//
	//
	public static junit.framework.Test suite() {
		final TestSuite suite = new TestSuite("");
		// List<Object> result = new ArrayList<Object>();
		final File dir = new File(pathToTests);
		final File[] files = dir.listFiles();
		//
		for (final File file : files) {
			if (file.isDirectory()) {
				if (!file.getName().equals(".svn")
						&& !file.getName().equals("Inner3Level")
						&& !file.getName().equals("ReadOnlySourcesTest"))
					suite.addTest(new SimpleTest(file.getName()));
			}
		}
		//
		return suite;
	}

	/*
	 * public Object[] testFactory() { List<Object> result = new ArrayList<Object>();
	 * File dir = new File(pathToTests); File[] files = dir.listFiles(); //
	 * for(File file : files) { if (file.isDirectory()) { result.add(new
	 * SimpleTest(file.getName())); } } // return result.toArray(); }
	 */
	public static class SimpleTest extends TestCase {

		private IWorkspace workspace = TranslationPlugin.getWorkspace();

		@Override
		@Before
		public void setUp() throws Exception {
			workspace = TranslationPlugin.getWorkspace();
		}

		private String tname = null;

		public SimpleTest(String name) {
			super(name);
			tname = name;
		}

		@Test
		public void testRun() {
			translationTest(tname);
		}

		@Override
		protected void runTest() throws Throwable {
			assertNotNull("TestCase.fName cannot be null", tname); // Some VMs
			// crash
			// when
			// calling
			// getMethod(null,null);
			Method runMethod = null;
			try {
				// use getMethod to get all public inherited
				// methods. getDeclaredMethods returns all
				// methods of this class but excludes the
				// inherited ones.
				runMethod = getClass().getMethod("testRun", (Class[]) null);
			} catch (final NoSuchMethodException e) {
				fail("Method \"" + "testRun" + "\" not found");
			}
			if (!Modifier.isPublic(runMethod.getModifiers())) {
				fail("Method \"" + "testRun" + "\" should be public");
			}

			try {
				runMethod.invoke(this);
			} catch (final InvocationTargetException e) {
				e.fillInStackTrace();
				throw e.getTargetException();
			} catch (final IllegalAccessException e) {
				e.fillInStackTrace();
				throw e;
			}
		}

		private void translationTest(String projectName) {
			try {
				final TestContext testContext = new TestContext(workspace);
				testContext.init(pathToTests + projectName + "/", projectName);
				//
				testContext.testAll();
				//
				// Assert.assertNotSame(testContext.getBuffer(),
				// testContext.getTransformedBuffer());
				//
				// String expected = "package tests.enums;public class
				// InitializerTest {public InitializerTest(){int a=2;}}";
				//
				// TODO :
				// Assert.assertEquals(testContext.getTransformedBuffer(),
				// expected);
				//
			} catch (final Exception e) {
				Assert.fail(e.getMessage() + " " + e.toString());
			}
		}
	}
	//
	//
	//

	/*
	 * private void translationTest(String projectName) { try { TestContext
	 * testContext = new TestContext(workspace); testContext.init(pathToTests +
	 * projectName + "/", projectName); // testContext.testAll(); // //
	 * Assert.assertNotSame(testContext.getBuffer(), //
	 * testContext.getTransformedBuffer()); // // String expected = "package
	 * tests.enums;public class // InitializerTest {public InitializerTest(){int
	 * a=2;}}"; // // TODO :
	 * Assert.assertEquals(testContext.getTransformedBuffer(), // expected); // }
	 * catch (Exception e) { Assert.fail(e.getMessage() + " " + e.toString()); } }
	 */
}
