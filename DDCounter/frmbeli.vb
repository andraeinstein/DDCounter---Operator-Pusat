﻿Imports MySql.Data.MySqlClient
Public Class frmbeli
    Dim rsl, rsl2, rsl3 As Integer

    Sub setujudiproses()
        'menampilkan data di datagrid yang dipilih
        Dim i As Integer = DataGridView1.CurrentRow.Index
        If txtnomor.Text = Nothing Then
            frmgagal.lblpesan.Text = "Isilah nomor HP terlebih dahulu !"
            frmgagal.ShowDialog()
        Else
            transaksi = "pulsa"
            frminfo.lblpesan.Text = "Isi pulsa " & lbloperator.Text & " " & DataGridView1.Item(0, i).Value & "  seharga Rp. " & DataGridView1.Item(2, i).Value & " ke nomor " & txtnomor.Text & "?"
            frminfo.ShowDialog()
        End If
    End Sub
    Private Sub frmbeli_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        lbldigit.Text = "Digit : " & Me.txtnomor.TextLength
    End Sub

    Private Sub btnlogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub SkypeButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton2.Click
        If lbloperator.Text = "PLN" Then
            Call setujudiproses()
        Else
            Call atur()
            SQLCari("select h.prefix,o.nama from harga.hlrid h,harga.harga ha,harga.operator o,harga.kartu k where h.prefix like '" & Microsoft.VisualBasic.Left(txtnomor.Text, 4) & "%' and o.nama='" & txtjenis.Text & "' and h.id_kartu=k.id_kartu and k.id_operator=o.id_operator")
            If objDataTable.Rows.Count > 0 Then
                SQLCari("select date(t.tgl_trx),tp.no_tujuan,tp.kode_produk from counter_pulsa.transaksi_pulsa tp,counter_pulsa.transaksi t where t.id_trx=tp.id_trx and tp.no_tujuan='" & txtnomor.Text & "' and tp.kode_produk='" & DataGridView1.Item(3, DataGridView1.CurrentRow.Index).Value & "' and DATE_SUB(CURDATE(),INTERVAL 0 DAY) <= tgl_trx")
                If objDataTable.Rows.Count > 0 Then
                    frmgagal.lblpesan.Text = "Nomor ini telah mengisi pulsa hari ini !"
                    frmgagal.ShowDialog()
                Else
                    Call setujudiproses()
                End If
            Else
                frmgagal.lblpesan.Text = "Nomor HP tidak cocok dengan Operator, Periksa ulang nomor anda !"
                frmgagal.ShowDialog()
            End If
        End If
    End Sub

    Private Sub txtnomor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtnomor.KeyPress
        'Textbox hanya boleh diisi angka
        If Char.IsDigit(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False Then
            e.Handled = True
        End If
        If e.KeyChar = Convert.ToChar(13) Then
            SkypeButton2_Click(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub SkypeButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton1.Click
        txtjenis.Text = Nothing
        txtnomor.Text = Nothing
        lbldigit.Text = "Digit : " & Me.txtnomor.TextLength
        lbloperator.Text = Nothing
        Me.Close()
    End Sub

    Private Sub txtnomor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtnomor.TextChanged
        lbldigit.Text = "Digit : " & Me.txtnomor.TextLength
    End Sub
End Class