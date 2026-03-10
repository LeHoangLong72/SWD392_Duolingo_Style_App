import apiClient from '../api/apiClient'

export async function getShopItems() {
  const response = await apiClient.get('/shop/items')
  return response?.data
}
