﻿using System;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using ShaderGen.Glsl;
using ShaderGen.Hlsl;
using ShaderGen.Metal;
using ShaderGen.Tests.Attributes;
using ShaderGen.Tests.Tools;
using Xunit;

namespace ShaderGen.Tests
{
    public class ShaderGeneratorTests
    {
        public static IEnumerable<object[]> ShaderSets()
        {
            yield return new object[] { "TestShaders.TestVertexShader.VS", null };
            yield return new object[] { null, "TestShaders.TestFragmentShader.FS" };
            yield return new object[] { "TestShaders.TestVertexShader.VS", "TestShaders.TestFragmentShader.FS" };
            yield return new object[] { null, "TestShaders.TextureSamplerFragment.FS" };
            yield return new object[] { "TestShaders.VertexAndFragment.VS", "TestShaders.VertexAndFragment.FS" };
            yield return new object[] { null, "TestShaders.ComplexExpression.FS" };
            yield return new object[] { "TestShaders.PartialVertex.VertexShaderFunc", null };
            yield return new object[] { "TestShaders.VeldridShaders.ForwardMtlCombined.VS", "TestShaders.VeldridShaders.ForwardMtlCombined.FS" };
            yield return new object[] { "TestShaders.VeldridShaders.ForwardMtlCombined.VS", null };
            yield return new object[] { null, "TestShaders.VeldridShaders.ForwardMtlCombined.FS" };
            yield return new object[] { "TestShaders.CustomStructResource.VS", null };
            yield return new object[] { "TestShaders.Swizzles.VS", null };
            yield return new object[] { "TestShaders.CustomMethodCalls.VS", null };
            yield return new object[] { "TestShaders.VeldridShaders.ShadowDepth.VS", "TestShaders.VeldridShaders.ShadowDepth.FS" };
            yield return new object[] { "TestShaders.ShaderBuiltinsTestShader.VS", null };
            yield return new object[] { null, "TestShaders.ShaderBuiltinsTestShader.FS" };
            yield return new object[] { "TestShaders.VectorConstructors.VS", null };
            yield return new object[] { "TestShaders.VectorIndexers.VS", null };
            yield return new object[] { "TestShaders.VectorStaticProperties.VS", null };
            yield return new object[] { "TestShaders.VectorStaticFunctions.VS", null };
            yield return new object[] { "TestShaders.MultipleResourceSets.VS", null };
            yield return new object[] { "TestShaders.MultipleColorOutputs.VS", "TestShaders.MultipleColorOutputs.FS" };
            yield return new object[] { "TestShaders.MultisampleTexture.VS", null };
            yield return new object[] { "TestShaders.BuiltInVariables.VS", null };
            yield return new object[] { "TestShaders.MathFunctions.VS", null };
            yield return new object[] { "TestShaders.Matrix4x4Members.VS", null };
            yield return new object[] { "TestShaders.CustomMethodUsingUniform.VS", null };
            yield return new object[] { "TestShaders.PointLightTestShaders.VS", null };
            yield return new object[] { "TestShaders.UIntVectors.VS", null };
            yield return new object[] { "TestShaders.VeldridShaders.UIntVertexAttribs.VS", null };
            yield return new object[] { "TestShaders.SwitchStatements.VS", null };
            yield return new object[] { "TestShaders.VariableTypes.VS", null };
            yield return new object[] { "TestShaders.OutParameters.VS", null };
            yield return new object[] { null, "TestShaders.ExpressionBodiedMethods.ExpressionBodyWithReturn" };
            yield return new object[] { null, "TestShaders.ExpressionBodiedMethods.ExpressionBodyWithoutReturn" };
            yield return new object[] { "TestShaders.StructuredBufferTestShader.VS", null };
            yield return new object[] { null, "TestShaders.DepthTextureSamplerFragment.FS" };
            yield return new object[] { null, "TestShaders.Enums.FS" };
            yield return new object[] { "TestShaders.VertexWithStructuredBuffer.VS", null };
        }

        public static IEnumerable<object[]> ComputeShaders()
        {
            yield return new object[] { "TestShaders.SimpleCompute.CS" };
            yield return new object[] { "TestShaders.ComplexCompute.CS" };
        }

        private static readonly HashSet<string> s_glslesSkippedShaders = new HashSet<string>()
        {
            "TestShaders.ComplexCompute.CS"
        };

        private void TestEndToEnd(Type backendType, string vsName, string fsName, string csName = null)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            ToolChain toolChain = ToolChain.Get(backendType);
            LanguageBackend backend = toolChain.GetBackend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            ShaderGenerationResult result = sg.GenerateShaders();
            IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
            Assert.Equal(1, sets.Count);
            GeneratedShaderSet set = sets[0];
            ShaderModel shaderModel = set.Model;

            if (!string.IsNullOrWhiteSpace(vsName))
            {
                ShaderFunction vsFunction = shaderModel.GetFunction(vsName);
                string vsCode = set.VertexShaderCode;
                toolChain.AssertCompilesCode(vsCode, Stage.Vertex, vsFunction.Name);
            }
            if (!string.IsNullOrWhiteSpace(fsName))
            {
                ShaderFunction fsFunction = shaderModel.GetFunction(fsName);
                string fsCode = set.FragmentShaderCode;
                toolChain.AssertCompilesCode(fsCode, Stage.Fragment, fsFunction.Name);
            }
            if (!string.IsNullOrWhiteSpace(csName))
            {
                ShaderFunction csFunction = shaderModel.GetFunction(csName);
                string fsCode = set.ComputeShaderCode;
                toolChain.AssertCompilesCode(fsCode, Stage.Compute, csFunction.Name);
            }
        }

        [HlslTheory]
        [MemberData(nameof(ShaderSets))]
        public void HlslEndToEnd(string vsName, string fsName)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            HlslBackend backend = new HlslBackend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            ShaderGenerationResult result = sg.GenerateShaders();
            IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
            Assert.Equal(1, sets.Count);
            GeneratedShaderSet set = sets[0];
            ShaderModel shaderModel = set.Model;

            if (vsName != null)
            {
                ShaderFunction vsFunction = shaderModel.GetFunction(vsName);
                string vsCode = set.VertexShaderCode;
                ToolChain.Hlsl.AssertCompilesCode(vsCode, Stage.Vertex, vsFunction.Name);
            }
            if (fsName != null)
            {
                ShaderFunction fsFunction = shaderModel.GetFunction(fsName);
                string fsCode = set.FragmentShaderCode;
                ToolChain.Hlsl.AssertCompilesCode(fsCode, Stage.Fragment, fsFunction.Name);
            }
        }

        [Glsl330Theory]
        [MemberData(nameof(ShaderSets))]
        public void Glsl330EndToEnd(string vsName, string fsName)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            Glsl330Backend backend = new Glsl330Backend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            ShaderGenerationResult result = sg.GenerateShaders();
            IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
            Assert.Equal(1, sets.Count);
            GeneratedShaderSet set = sets[0];
            ShaderModel shaderModel = set.Model;

            if (vsName != null)
            {
                ShaderFunction vsFunction = shaderModel.GetFunction(vsName);
                string vsCode = set.VertexShaderCode;
                ToolChain.Glsl330.AssertCompilesCode(vsCode, Stage.Vertex, vsFunction.Name);
            }
            if (fsName != null)
            {
                ShaderFunction fsFunction = shaderModel.GetFunction(fsName);
                string fsCode = set.FragmentShaderCode;
                ToolChain.Glsl330.AssertCompilesCode(fsCode, Stage.Fragment, fsFunction.Name);
            }
        }

        [GlslEs300Theory]
        [MemberData(nameof(ShaderSets))]
        public void GlslEs300EndToEnd(string vsName, string fsName)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            GlslEs300Backend backend = new GlslEs300Backend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            ShaderGenerationResult result = sg.GenerateShaders();
            IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
            Assert.Equal(1, sets.Count);
            GeneratedShaderSet set = sets[0];
            ShaderModel shaderModel = set.Model;

            if (vsName != null)
            {
                ShaderFunction vsFunction = shaderModel.GetFunction(vsName);
                string vsCode = set.VertexShaderCode;
                GlsLangValidatorTool.AssertCompilesCode(vsCode, "vert", false);
            }
            if (fsName != null)
            {
                ShaderFunction fsFunction = shaderModel.GetFunction(fsName);
                string fsCode = set.FragmentShaderCode;
                GlsLangValidatorTool.AssertCompilesCode(fsCode, "frag", false);
            }
        }

        [Glsl450Theory]
        [MemberData(nameof(ShaderSets))]
        public void Glsl450EndToEnd(string vsName, string fsName)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            LanguageBackend backend = new Glsl450Backend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            ShaderGenerationResult result = sg.GenerateShaders();
            IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
            Assert.Equal(1, sets.Count);
            GeneratedShaderSet set = sets[0];
            ShaderModel shaderModel = set.Model;

            if (vsName != null)
            {
                ShaderFunction vsFunction = shaderModel.GetFunction(vsName);
                string vsCode = set.VertexShaderCode;
                GlsLangValidatorTool.AssertCompilesCode(vsCode, "vert", true);
            }
            if (fsName != null)
            {
                ShaderFunction fsFunction = shaderModel.GetFunction(fsName);
                string fsCode = set.FragmentShaderCode;
                GlsLangValidatorTool.AssertCompilesCode(fsCode, "frag", true);
            }
        }

        [MetalTheory]
        [MemberData(nameof(ShaderSets))]
        public void MetalEndToEnd(string vsName, string fsName)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            LanguageBackend backend = new MetalBackend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            ShaderGenerationResult result = sg.GenerateShaders();
            IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
            Assert.Equal(1, sets.Count);
            GeneratedShaderSet set = sets[0];
            ShaderModel shaderModel = set.Model;

            if (vsName != null)
            {
                ShaderFunction vsFunction = shaderModel.GetFunction(vsName);
                string vsCode = set.VertexShaderCode;
                MetalTool.AssertCompilesCode(vsCode);
            }
            if (fsName != null)
            {
                ShaderFunction fsFunction = shaderModel.GetFunction(fsName);
                string fsCode = set.FragmentShaderCode;
                MetalTool.AssertCompilesCode(fsCode);
            }
        }
        
        [BackendFact(typeof(HlslBackend), typeof(GlslEs300Backend), typeof(Glsl330Backend), typeof(Glsl450Backend), typeof(MetalBackend))]
        public void AllSetsEndToEnd()
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            LanguageBackend[] backends = new LanguageBackend[]
            {
                new HlslBackend(compilation),
                new GlslEs300Backend(compilation),
                new Glsl330Backend(compilation),
                new Glsl450Backend(compilation),
                new MetalBackend(compilation),
            };
            ShaderGenerator sg = new ShaderGenerator(compilation, backends);

            ShaderGenerationResult result = sg.GenerateShaders();

            foreach (LanguageBackend backend in backends)
            {
                IReadOnlyList<GeneratedShaderSet> sets = result.GetOutput(backend);
                foreach (GeneratedShaderSet set in sets)
                {
                    if (set.VertexShaderCode != null)
                    {
                        if (backend is HlslBackend)
                        {
                            ToolChain.Hlsl.AssertCompilesCode(set.VertexShaderCode, Stage.Vertex, set.VertexFunction.Name);
                        }
                        else if (backend is MetalBackend)
                        {
                            MetalTool.AssertCompilesCode(set.VertexShaderCode);
                        }
                        else
                        {
                            bool is450 = backend is Glsl450Backend;
                            GlsLangValidatorTool.AssertCompilesCode(set.VertexShaderCode, "vert", is450);
                        }
                    }
                    if (set.FragmentFunction != null)
                    {
                        if (backend is HlslBackend)
                        {
                            ToolChain.Hlsl.AssertCompilesCode(set.FragmentShaderCode, Stage.Fragment, set.FragmentFunction.Name);
                        }
                        else if (backend is MetalBackend)
                        {
                            MetalTool.AssertCompilesCode(set.FragmentShaderCode);
                        }
                        else
                        {
                            bool is450 = backend is Glsl450Backend;
                            GlsLangValidatorTool.AssertCompilesCode(set.FragmentShaderCode, "frag", is450);
                        }
                    }
                    if (set.ComputeFunction != null)
                    {
                        if (backend is HlslBackend)
                        {
                            ToolChain.Hlsl.AssertCompilesCode(set.ComputeShaderCode, Stage.Compute, set.ComputeFunction.Name);
                        }
                        else if (backend is MetalBackend)
                        {
                            MetalTool.AssertCompilesCode(set.ComputeShaderCode);
                        }
                        else
                        {
                            if (backend is GlslEs300Backend)
                            {
                                string fullname = set.ComputeFunction.DeclaringType + "." + set.ComputeFunction.Name;
                                if (s_glslesSkippedShaders.Contains(fullname))
                                {
                                    continue;
                                }
                            }

                            bool is450 = backend is Glsl450Backend;
                            GlsLangValidatorTool.AssertCompilesCode(set.ComputeShaderCode, "comp", is450);
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> ErrorSets()
        {
            yield return new object[] { "TestShaders.MissingFunctionAttribute.VS", null };
            yield return new object[] { "TestShaders.PercentOperator.PercentVS", null };
            yield return new object[] { "TestShaders.PercentOperator.PercentEqualsVS", null };
        }

        [Theory]
        [MemberData(nameof(ErrorSets))]
        public void ExpectedException(string vsName, string fsName)
        {
            Compilation compilation = TestUtil.GetTestProjectCompilation();
            Glsl330Backend backend = new Glsl330Backend(compilation);
            ShaderGenerator sg = new ShaderGenerator(
                compilation,
                vsName,
                fsName,
                backend);

            Assert.Throws<ShaderGenerationException>(() => sg.GenerateShaders());
        }
    }
}
