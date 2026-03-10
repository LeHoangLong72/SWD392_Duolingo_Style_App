import apiClient from '../api/apiClient'

export async function getLearningPath() {
  const response = await apiClient.get('/learning/japanese-path')
  return response?.data
}

export async function startLesson(lessonId) {
  const response = await apiClient.post(`/lesson-content/start/${lessonId}`)
  return response?.data
}
