Imports MySql.Data.MySqlClient
Public Class frmlogin

    Private Sub SkypeButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton1.Click
        If txtid.Text = Nothing And txtpin.Text = Nothing Then
            MessageBox.Show("Data tidak lengkap !", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            'Try
            Call atur()
            SQLCari("select id_member, nama, alamat, pin, saldo, status from counter_pulsa.federated_member where id_member='" & txtid.Text & "' and pin='" & txtpin.Text & "' and status=1")
            If objDataTable.Rows.Count > 0 Then
                frmsukses.lblpesan.Text = "Selamat datang " & objDataTable.Rows(0).Item("nama").ToString & " !!"
                frmsukses.ShowDialog()
                frmdepo.txtid.Text = objDataTable.Rows(0).Item("id_member").ToString
                frmdepo.txtnama.Text = objDataTable.Rows(0).Item("nama").ToString
                frmdepo.txtalamat.Text = objDataTable.Rows(0).Item("alamat").ToString
                txtid.Text = Nothing
                txtpin.Text = Nothing
                Me.Close()
                frmdepo.ShowDialog()
            Else
                MessageBox.Show("Maaf, ID atau Pin anda salah !", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            'Catch ex As Exception
            'End Try
        End If
    End Sub

    Private Sub SkypeButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton2.Click
        Me.Close()
    End Sub

    Private Sub frmlogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtid.Focus()
        Me.SuspendLayout()
        Me.ResumeLayout()
    End Sub

    Private Sub txtid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtid.KeyPress
        If e.KeyChar = Convert.ToChar(13) Then
            SkypeButton1_Click(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub txtpin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtpin.KeyPress
        If e.KeyChar = Convert.ToChar(13) Then
            SkypeButton1_Click(Me, EventArgs.Empty)
        End If
    End Sub
End Class