Imports MySql.Data.MySqlClient
Public Class frminfo

    Private Sub SkypeButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton1.Click
        If transaksi = "pulsa" Then
            urutid = urutid + 1
            Call atur()
            SQLCari("select id_trx from counter_pulsa.transaksi order by id_trx desc limit 1")
            If objDataTable.Rows.Count > 0 Then
                idtrxpul = objDataTable.Rows(0).Item("id_trx").ToString + urutid
            End If
            SQLQuery("insert into harga.tampung(id_trx,kode_produk,no_tujuan,harga) values(" & idtrxpul & ",'" & frmbeli.DataGridView1.Item(3, frmbeli.DataGridView1.CurrentRow.Index).Value & "','" & frmbeli.txtnomor.Text & "'," & frmbeli.DataGridView1.Item(2, frmbeli.DataGridView1.CurrentRow.Index).Value & ")")
            frmlagi.ShowDialog()
            Me.Close()
        Else
            Call atur()
            SQLQuery("update harga.akses set ket='depo'")
            SQLQuery("update harga.tampung2 set idres='" & frmdepo.txtid.Text & "', nama='" & frmdepo.txtnama.Text & "', depo=" & frmdepo.txtjumlah.Text & "")
            frmbayar.lblpesan.Text = "Silakan bayar Rp " & frmdepo.txtjumlah.Text & ",- ke kasir !"
            frmbayar.ShowDialog()
            Me.Close()
        End If
    End Sub

    Private Sub SkypeButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton2.Click
        Me.Close()
    End Sub

End Class