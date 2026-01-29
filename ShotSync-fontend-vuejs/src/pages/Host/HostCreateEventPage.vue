<template>
  <q-page class="q-pa-md row justify-center">
    <q-form @submit.prevent="submitForm" class="q-gutter-md col-8">
      <q-input v-model="form.tournamentName" label="Tên giải đấu" outlined required />

      <q-input v-model="form.slogan" type="textarea" label="Slogan(Optional)" outlined autogrow />

      <q-input v-model="form.venue" label="Tên câu lạc bộ" outlined required />

      <q-input v-model="form.location" label="Địa chỉ tổ chức" outlined required />

      <q-input v-model="form.date" label="Ngày tổ chức" outlined type="date" required />

      <q-input v-model="form.time" label="Giờ bắt đầu" outlined type="time" required />

      <q-select
        v-model="form.format"
        :options="formats"
        label="Thể thức thi đấu"
        outlined
        required
      />

      <q-input
        v-model.number="form.fee"
        type="number"
        label="Lệ phí tham gia (VNĐ)"
        outlined
        required
      />

      <q-input
        v-model.number="form.profit"
        type="number"
        label="Tổng giải thưởng (VNĐ)"
        outlined
        required
      />

      <q-input
        v-model.number="form.maxParticipants"
        type="number"
        label="Số lượng người tham gia tối đa"
        outlined
        required
      />

      <q-editor v-model="form.description" min-height="5rem" />

      <div class="row justify-end">
        <q-btn label="Tạo giải đấu" type="submit" color="primary" />
      </div>
    </q-form>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue'

import EventService from 'src/services/event.service'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import ROUTE_PATHS from 'src/router/route-paths'
import type { Event as AppEvent } from 'src/types/schema' // Alias to avoid collision with global Event

const router = useRouter()
const $q = useQuasar()

const form = ref({
  tournamentName: '',
  slogan: '',
  venue: '',
  location: '',
  date: '',
  time: '',
  format: 'Single Elimination', // Default value
  fee: null,
  profit: null,
  maxParticipants: null,
  description: 'Nhập mô tả về giải đấu...',
})

const formats = [
  'Single Elimination',
  'Double Elimination',
  'Round Robin',
  'Swiss',
  'Free for all',
  'Leaderboard',
]

async function submitForm() {
    try {
        // Combine date and time
        const dateTimeString = `${form.value.date}T${form.value.time}:00`;
        const dateObj = new Date(dateTimeString);

        const payload = {
            name: form.value.tournamentName,
            venue: form.value.venue,
            location: form.value.location,
            date: dateObj.toISOString(),
            numberOfPlayers: form.value.maxParticipants || 32,
            slogan: form.value.slogan,
            format: form.value.format,
            entryFee: form.value.fee || 0,
            totalPrize: form.value.profit || 0,
            description: form.value.description
        };

        await EventService.createEvent(payload as unknown as AppEvent); // Cast payload to AppEvent

        $q.notify({
            color: 'positive',
            message: 'Tạo giải đấu thành công!',
            icon: 'check',
            position: 'top'
        });

        // Redirect to Host Profile or Event List
        await router.push(ROUTE_PATHS.HOST.PROFILE);
    } catch (error) {
        console.error('Error creating event:', error);
        $q.notify({
            color: 'negative',
            message: 'Có lỗi xảy ra khi tạo giải đấu.',
            icon: 'report_problem',
            position: 'top'
        });
    }
}
</script>
