namespace Aula3D.VisionCore.Interfaces
{
    /// <summary>
    /// Contrato base acordado entre as duplas no Dia 1.
    /// A Dupla 1 implementa este contrato em GestorDeVisaoFacade.
    /// A Dupla 2 implementa este contrato em MouseMock (adaptador de testes).
    /// </summary>
    public interface IGestureProvider
    {
        /// <summary>Coordenada X normalizada do centro de massa da mão (pixels da câmera).</summary>
        float X { get; }

        /// <summary>Coordenada Y normalizada do centro de massa da mão (pixels da câmera).</summary>
        float Y { get; }

        /// <summary>
        /// Gesto detectado no momento.
        /// true  = mão ABERTA  (para a Dupla 2: acionar rotação livre do modelo 3D)
        /// false = mão FECHADA (para a Dupla 2: acionar translação/zoom do modelo 3D)
        /// </summary>
        bool GestoDetectado { get; }

        /// <summary>Indica se a câmera/mouse está com rastreamento ativo.</summary>
        bool HandDetected { get; }
    }
}
