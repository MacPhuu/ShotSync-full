<template>
  <q-page class="q-pa-md">
    <q-table title="Hosts List" :rows="rows" :columns="columns" row-key="id" flat bordered>
      <template v-slot:body-cell-actions="props">
        <q-td align="center">
          <q-btn flat round icon="edit" color="primary" @click="editHost(props.row)" />
          <q-btn flat round icon="delete" color="negative" @click="deleteHost(props.row)" />
        </q-td>
      </template>
    </q-table>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { QTableColumn } from 'quasar'

interface Host {
  id: number
  index: number
  name: string
  created_at: string
  status: string
}

const columns: QTableColumn<Host>[] = [
  {
    name: 'index',
    label: 'No.',
    align: 'left',
    field: (row) => row.index,
  },
  {
    name: 'name',
    label: 'Host Name',
    align: 'left',
    field: 'name',
  },
  {
    name: 'created_at',
    label: 'Created Date',
    align: 'left',
    field: 'created_at',
  },
  {
    name: 'status',
    label: 'Status',
    align: 'left',
    field: 'status',
  },
  {
    name: 'actions',
    label: 'Actions',
    align: 'center',
    field: 'id', // dummy field to satisfy type
  },
]

const rows = ref<Host[]>([
  { id: 1, index: 1, name: 'Host A', created_at: '2024-01-01', status: 'Active' },
  { id: 2, index: 2, name: 'Host B', created_at: '2024-02-15', status: 'Inactive' },
  { id: 3, index: 3, name: 'Host C', created_at: '2024-03-10', status: 'Active' },
])

function editHost(row: Host) {
  console.log('Edit host:', row)
}

function deleteHost(row: Host) {
  console.log('Delete host:', row)
}
</script>
