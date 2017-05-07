﻿// ReSharper disable MissingXmlDoc
namespace ApkReverse.Core.SourcesTranslator.Smali.SmaliParser
{
    /// <summary>
    /// 
    /// </summary>
    public enum TokenType
    {
        ClassDirective,
        SuperDirective,
        ImplementsDirective,
        SourceDirective,
        FieldDirective,
        EndFieldDirective,
        SubannotationDirective,
        EndSubannotationDirective,
        AnnotationDirective,
        EndAnnotationDirective,
        EnumDirective,
        MethodDirective,
        EndMethodDirective,
        RegistersDirective,
        LocalsDirective,
        ArrayDataDirective,
        EndArrayDataDirective,
        PackedSwitchDirective,
        EndPackedSwitchDirective,
        SparseSwitchDirective,
        EndSparseSwitchDirective,
        CatchDirective,
        CatchallDirective,
        LineDirective,
        ParameterDirective,
        EndParameterDirective,
        LocalDirective,
        EndLocalDirective,
        RestartLocalDirective,
        PrologueDirective,
        EpilogueDirective,
        
        IntegerLiteral,
        NegativeIntegerLiteral,
        LongLiteral,
        ShortLiteral,
        ByteLiteral,
        FloatLiteralOrId,
        DoubleLiteralOrId,
        FloatLiteral,
        DoubleLiteral,
        BoolLiteral,
        NullLiteral,
        StringLiteral,
        CharLiteral,

        ParamListOrIdPrimitiveType,

        PrimitiveType,
        ClassDescriptor,
        ArrayTypePrefix,
        VoidType,
        SimpleName,
        MemberName,

        Register,
        AnnotationVisibility,
        AccessSpec,
        VerificationErrorType,
        InlineIndex,
        VtableIndex,
        FieldOffset,
        LineComment,

        Dotdot,
        Arrow,
        Equal,
        Colon,
        Comma,
        OpenBrace,
        CloseBrace,
        OpenParen,
        CloseParen,
        WhiteSpace,
        Eof,

        INSTRUCTION_FORMAT10t,
        INSTRUCTION_FORMAT10x,
        INSTRUCTION_FORMAT11n,
        INSTRUCTION_FORMAT11x,
        INSTRUCTION_FORMAT12x_OR_ID,
        INSTRUCTION_FORMAT12x,
        INSTRUCTION_FORMAT20bc,
        INSTRUCTION_FORMAT20t,
        INSTRUCTION_FORMAT21c_FIELD,
        INSTRUCTION_FORMAT21c_STRING,
        INSTRUCTION_FORMAT21c_TYPE,
        INSTRUCTION_FORMAT21ih,
        INSTRUCTION_FORMAT21lh,
        INSTRUCTION_FORMAT21s,
        INSTRUCTION_FORMAT21t,
        INSTRUCTION_FORMAT22b,
        INSTRUCTION_FORMAT22c_FIELD,
        INSTRUCTION_FORMAT22c_TYPE,
        INSTRUCTION_FORMAT22cs_FIELD,
        INSTRUCTION_FORMAT22s_OR_ID,
        INSTRUCTION_FORMAT22s,
        INSTRUCTION_FORMAT22t,
        INSTRUCTION_FORMAT22x,
        INSTRUCTION_FORMAT23x,
        INSTRUCTION_FORMAT30t,
        INSTRUCTION_FORMAT31c,
        INSTRUCTION_FORMAT31i_OR_ID,
        INSTRUCTION_FORMAT31i,
        INSTRUCTION_FORMAT31t,
        INSTRUCTION_FORMAT32x,
        INSTRUCTION_FORMAT35c_METHOD,
        INSTRUCTION_FORMAT35c_TYPE,
        INSTRUCTION_FORMAT35mi_METHOD,
        INSTRUCTION_FORMAT35ms_METHOD,
        INSTRUCTION_FORMAT3rc_METHOD,
        INSTRUCTION_FORMAT3rc_TYPE,
        INSTRUCTION_FORMAT3rmi_METHOD,
        INSTRUCTION_FORMAT3rms_METHOD,
        INSTRUCTION_FORMAT45cc_METHOD,
        INSTRUCTION_FORMAT4rcc_METHOD,
        INSTRUCTION_FORMAT51l

    }
}