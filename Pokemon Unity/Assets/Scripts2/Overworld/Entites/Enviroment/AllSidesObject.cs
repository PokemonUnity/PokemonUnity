Public Class AllSidesObject

    Inherits Entity

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, True)
    End Sub

End Class