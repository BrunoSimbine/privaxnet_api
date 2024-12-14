using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using privaxnet_api.Models;
using System.Threading.Tasks;

namespace privaxnet_api.Repository.MessageRepository;

public class MessageRepository : IMessageRepository
{
	private readonly HttpClient _httpClient;
	public MessageRepository(IHttpClientFactory httpClientFactory){
		_httpClient = httpClientFactory.CreateClient("WhatsappClient");
	}


	public async Task<string> SendVoucherAsync(MessageVoucher voucher) {

		var request = voucher;
		request.DataAmount = voucher.DataAmount / 1024;
		string jsonData = JsonSerializer.Serialize(voucher);
		var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync("Message/send/voucher", content);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}


	public async Task<string> SendWelcomeAsync(MessageUser user) {

		string jsonData = JsonSerializer.Serialize(user);

		var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync("Message/send/welcome", content);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}
}