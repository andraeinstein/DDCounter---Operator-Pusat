Imports MySql.Data.MySqlClient
Public Class frmexit
    Private Sub SkypeButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton1.Click
        Call atur()
        SQLCari("select id,pwd from harga.pengguna where pwd='" & computeHash(txtpwd.Text) & "' and id=1")
        If objDataTable.Rows.Count > 0 Then
            If restartatauexit = "X" Then
                Application.Exit()
            Else
                Me.Close()
                frmMenu.WindowState = FormWindowState.Minimized
            End If
        Else
            MessageBox.Show("Password salah !!", "Set Password", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        txtpwd.Text = ""
    End Sub

    Private Sub SkypeButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton2.Click
        Me.Close()
    End Sub

    Private Sub txtpwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtpwd.KeyPress
        If e.KeyChar = Convert.ToChar(13) Then
            SkypeButton1_Click(Me, EventArgs.Empty)
        End If
    End Sub
End Class