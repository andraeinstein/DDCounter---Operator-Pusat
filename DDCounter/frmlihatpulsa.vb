Imports MySql.Data.MySqlClient
Public Class frmlihatpulsa

    Private Sub frmlihatpulsa_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call atur()
        SQLCari("select t.id_trx as ID, o.nama as Operator, h.denom as Nominal, t.no_tujuan as Nomor" _
                                & ", t.harga as Harga from harga.tampung t, harga.harga h, harga.operator o where t.kode_produk = h.kode_produk " _
                                & "and h.id_operator=o.id_operator")
        DataGridView1.DataSource = objDataTable
    End Sub

    Private Sub SkypeButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton1.Click
        Me.Close()
    End Sub
End Class