<template>
  <q-page class="bg-grey-1 q-pa-md">
    <div class="q-mx-auto" style="max-width: 1200px">
      <!-- Tiêu đề -->
      <div class="row justify-between items-center">
        <div class="text-h4 text-weight-bold">Tin tức nổi bật</div>
        <q-btn icon="refresh" flat label="Làm mới" @click="fetchNews" />
      </div>

      <!-- Bài viết nổi bật -->
      <q-card v-if="featuredNews" flat class="my-card bg-white">
        <q-card-section class="q-pa-sm">
          <q-img :src="featuredNews.image" :alt="featuredNews.title" :ratio="21 / 9" />
        </q-card-section>
        <q-card-section>
          <div class="text-h6">{{ featuredNews.title }}</div>
          <div class="text-caption text-grey">{{ featuredNews.date }}</div>
          <div class="q-mt-sm">{{ featuredNews.description }}</div>
        </q-card-section>
        <q-card-actions>
          <q-btn flat dense label="View more" />
        </q-card-actions>
      </q-card>

      <q-separator spaced />

      <!-- Danh sách tin tức -->
      <div class="row q-col-gutter-md">
        <div v-for="(item, index) in otherNews" :key="index" class="col-xs-12 col-sm-6 col-md-4">
          <q-card flat bordered class="bg-white">
            <q-card-section class="q-pa-sm">
              <q-img :src="item.image" :alt="item.title" :ratio="16 / 9" />
            </q-card-section>
            <q-card-section>
              <div class="text-subtitle1">{{ item.title }}</div>
              <div class="text-caption text-grey">{{ item.date }}</div>
            </q-card-section>

            <q-card-actions>
              <q-btn flat dense label="View more" @click="viewNews(item)" />
            </q-card-actions>
          </q-card>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface NewsItem {
  title: string
  date: string
  image: string
  description: string
}

const featuredNews = ref<NewsItem | null>({
  title: 'Quyết Chiến, Phương Vinh bùng nổ ở giải Billiards quốc tế',
  date: '22/06/2025',
  image:
    'https://i1-thethao.vnecdn.net/2025/05/30/Tra-n-Quye-t-Chie-n-JPG-174861-8179-4979-1748618205.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=yKzry8NGasLOn7UQ0UZiwA',
  description:
    'Các đại diện Việt Nam thi đấu xuất sắc để loại ba cơ thủ hàng đầu thế giới ở giải Billiards carom 3 băng quốc tế - Cup SCTV với tổng giải thưởng gần một tỷ đồng....',
})

const otherNews = ref<NewsItem[]>([
  {
    title: 'Việt Nam vào chung kết billiard đồng đội thế giới',
    date: '21/06/2025',
    image:
      'https://i1-thethao.vnecdn.net/2025/03/16/bao-phu-o-ng-vinh-1742124573-1-5178-7793-1742124933.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=LuQgO1OImqEbr-WtZipAyQ',
    description:
      'Đức- Trần Quyết Chiến và Bao Phương Vinh cùng thắng đậm Bỉ chiều nay 16/3, để lần thứ hai liên tiếp vào chung kết carom 3 băng đồng đội thế...',
  },
  {
    title: 'Hoàng "Sao" thắng áp đảo cơ thủ Philippines ở giải bi-a 700 triệu đồng',
    date: '20/06/2025',
    image:
      'https://cdn.24h.com.vn/upload/2-2025/images/2025-06-17/Hoang-hoangduong1-1750153922-546-width740height495.jpg',
    description:
      'Với vị thế của cơ thủ số 25 thế giới, Hoàng "Sao" không khó để đánh bại đối thủ Philippines.',
  },
  {
    title: '"Nữ hoàng bi-a carom” Yến Nhi tiến sâu ở giải Hàn Quốc, đua phần thưởng 700 triệu đồng',
    date: '19/06/2025',
    image:
      'https://cdn.24h.com.vn/upload/2-2025/images/2025-06-17/nhi-1-1750169722-770-width740height504.jpg',
    description:
      'Nguyễn Hoàng Yến Nhi đang thể hiện phong độ ấn tượng, vượt qua nhiều cơ thủ mạnh để tiến sâu tại LPBA Championship 2025.',
  },
])

const fetchNews = () => {
  // TODO: Tải lại danh sách tin tức từ API nếu có
}

const viewNews = (item: NewsItem) => {
  // TODO: Điều hướng đến trang chi tiết tin tức
  console.log('Đọc bài:', item.title)
}
</script>

<style scoped>
.my-card {
  border-radius: 8px;
  overflow: hidden;
}
</style>
