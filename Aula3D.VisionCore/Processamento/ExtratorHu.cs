using OpenCvSharp;

namespace Aula3D.VisionCore.Processamento
{
    /// <summary>
    /// Etapa 2 do pipeline PDI: calcula o contorno, o centro de massa
    /// e os 7 Momentos de Hu do contorno da mão.
    ///
    /// Os Momentos de Hu são invariantes a escala, rotação e translação —
    /// ideal para reconhecer a "forma" da mão independente de posição.
    /// </summary>
    public static class ExtratorHu
    {
        /// <summary>
        /// Calcula o centro de massa e o bounding rect a partir de <paramref name="contour"/>.
        /// Preenche <see cref="HandTrackingResult.CenterOfMass"/> e <see cref="HandTrackingResult.BoundingRect"/>.
        /// </summary>
        public static void ExtrairGeometria(Point[] contour, HandTrackingResult resultado)
        {
            resultado.BoundingRect = Cv2.BoundingRect(contour);

            Moments m = Cv2.Moments(contour);
            if (m.M00 > 0)
            {
                resultado.CenterOfMass = new Point(
                    (int)(m.M10 / m.M00),
                    (int)(m.M01 / m.M00)
                );
            }
        }

        // ----------------------------------------------------------------
        // TODO - Dupla 1: Implementar os 7 Momentos de Hu
        // ----------------------------------------------------------------
        // Os Momentos de Hu são derivados dos momentos centrais normalizados
        // (µpq / µ00^((p+q)/2+1)) e fornecem uma "impressão digital" da forma.
        //
        // Passos sugeridos:
        //  1. Calcular os momentos de 2ª e 3ª ordem com Cv2.Moments(contour).
        //  2. Chamar Cv2.HuMoments(moments) → double[7].
        //  3. Aplicar transformação log: -sign(h) * log10(|h| + 1e-10) para
        //     normalizar a grande variação de escala.
        //  4. Armazenar o vetor resultante no campo HuDescriptor abaixo.
        //
        // Exemplo de uso posterior (ClassificadorDeGestos):
        //   double distancia = CalcularDistanciaEuclidiana(descAtual, descGravado);
        //   if (distancia < limiar) gestureId = id_do_gesto;
        // ----------------------------------------------------------------

        /// <summary>
        /// [TODO - Dupla 1] Calcula e retorna o vetor de 7 Momentos de Hu
        /// com escala logarítmica para o <paramref name="contour"/> fornecido.
        /// Retorna array de zeros enquanto não implementado.
        /// </summary>
        public static double[] CalcularMomentosHu(Point[] contour)
        {
            // TODO: substituir pelo cálculo real dos Momentos de Hu
            // Sugestão:
            //   var mom = Cv2.Moments(contour);
            //   double[] hu = new double[7];
            //   Cv2.HuMoments(mom, hu);
            //   for (int i = 0; i < 7; i++)
            //       hu[i] = -Math.Sign(hu[i]) * Math.Log10(Math.Abs(hu[i]) + 1e-10);
            //   return hu;
            return new double[7];
        }
    }
}
