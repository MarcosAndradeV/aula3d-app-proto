#nullable enable
using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace Aula3D.VisionCore.Processamento
{
    /// <summary>
    /// Etapa 3 do pipeline PDI: decide se a mão está ABERTA ou FECHADA
    /// a partir dos defeitos de convexidade, e expõe um ponto de extensão
    /// para comparação com assinaturas pré-gravadas (Momentos de Hu).
    /// </summary>
    public static class ClassificadorDeGestos
    {
        /// <summary>
        /// Classifica o gesto e preenche <see cref="HandTrackingResult.IsHandOpen"/>,
        /// <see cref="HandTrackingResult.State"/> e <see cref="HandTrackingResult.DefectPoints"/>
        /// a partir dos defeitos de convexidade do <paramref name="contour"/>.
        /// </summary>
        public static void Classificar(Point[] contour, HandTrackingResult resultado)
        {
            int[] hullIndices = Cv2.ConvexHullIndices(contour);
            var defectPoints = new List<Point>();
            int defectCount  = 0;

            if (hullIndices.Length > 3 && contour.Length > 3)
            {
                Vec4i[] defects = Cv2.ConvexityDefects(contour, hullIndices);

                foreach (var defect in defects)
                {
                    double depth    = defect.Item3 / 256.0;
                    double minDepth = Math.Max(resultado.BoundingRect.Height * 0.15, 20);

                    if (depth > minDepth)
                    {
                        Point start        = contour[defect.Item0];
                        Point end          = contour[defect.Item1];
                        Point farthestPoint = contour[defect.Item2];

                        // Lei dos cossenos para medir o ângulo no defeito
                        double a = Dist(end, start);
                        double b = Dist(farthestPoint, start);
                        double c = Dist(end, farthestPoint);

                        if (b > 0 && c > 0)
                        {
                            double angle = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
                            if (angle <= Math.PI / 2.0)
                            {
                                defectCount++;
                                defectPoints.Add(farthestPoint);
                            }
                        }
                    }
                }
            }

            double aspectRatio =
                Math.Max(resultado.BoundingRect.Height, resultado.BoundingRect.Width) /
                (double)Math.Min(resultado.BoundingRect.Height, resultado.BoundingRect.Width);

            resultado.DefectPoints = defectPoints.ToArray();
            resultado.IsHandOpen   = defectCount >= 3 || (defectCount < 3 && aspectRatio > 1.35);
            resultado.State        = resultado.IsHandOpen ? "ABERTA" : "FECHADA";
        }

        // ----------------------------------------------------------------
        // TODO - Dupla 1: Implementar reconhecimento por assinaturas gravadas
        // ----------------------------------------------------------------
        // O método abaixo deve:
        //  1. Receber o vetor de 7 Momentos de Hu calculado por ExtratorHu.
        //  2. Comparar com um dicionário de assinaturas pré-gravadas
        //     (gravar em arquivo JSON ou embedded resource).
        //  3. Retornar o ID do gesto mais próximo se a distância for < limiar,
        //     ou null se nenhum gesto reconhecido.
        //
        // Assinaturas podem ser gravadas rodando VisionConsole em "modo treino":
        //   gravadas["ABERTA"]  = momentosHuAtual;
        //   gravadas["FECHADA"] = momentosHuAtual;
        //   File.WriteAllText("gestures.json", JsonSerializer.Serialize(gravadas));
        // ----------------------------------------------------------------

        /// <summary>
        /// [TODO - Dupla 1] Compara <paramref name="momentosHu"/> com assinaturas
        /// pré-gravadas e retorna o nome do gesto reconhecido, ou null.
        /// </summary>
        public static string? ReconhecerPorAssinatura(double[] momentosHu)
        {
            // TODO: carregar assinaturas de gestures.json e comparar por distância Euclidiana
            return null;
        }

        // -- helpers --

        private static double Dist(Point a, Point b) =>
            Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}
