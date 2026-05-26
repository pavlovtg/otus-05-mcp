"""E2E-тесты MCP tools: list_apis, search_apis."""
import httpx
import pytest
from conftest import initialize_session, mcp_request, parse_sse_response


@pytest.fixture(scope="module")
def session(mcp_client: httpx.Client) -> str | None:
    return initialize_session(mcp_client)


def call_tool(client: httpx.Client, session_id: str | None, tool_name: str, arguments: dict | None = None) -> dict:
    payload = mcp_request(
        "tools/call",
        {"name": tool_name, "arguments": arguments or {}},
        request_id=2,
    )
    headers = {"Accept": "application/json, text/event-stream"}
    if session_id:
        headers["mcp-session-id"] = session_id
    response = client.post("/", json=payload, headers=headers)
    response.raise_for_status()
    return parse_sse_response(response)


def list_tools(client: httpx.Client, session_id: str | None) -> dict:
    payload = mcp_request("tools/list", {}, request_id=2)
    headers = {"Accept": "application/json, text/event-stream"}
    if session_id:
        headers["mcp-session-id"] = session_id
    response = client.post("/", json=payload, headers=headers)
    response.raise_for_status()
    return parse_sse_response(response)


def test_list_apis_tool_exists(mcp_client: httpx.Client, session: str | None) -> None:
    """list_apis и search_apis должны быть в списке tools."""
    result = list_tools(mcp_client, session)
    assert "result" in result, f"Unexpected response: {result}"
    tools = result["result"]["tools"]
    tool_names = [t["name"] for t in tools]
    assert "list_apis" in tool_names, f"list_apis not found in {tool_names}"
    assert "search_apis" in tool_names, f"search_apis not found in {tool_names}"


def test_list_apis_returns_contracts(mcp_client: httpx.Client, session: str | None) -> None:
    """list_apis должен вернуть непустой список контрактов."""
    result = call_tool(mcp_client, session, "list_apis")
    assert "result" in result, f"Unexpected response: {result}"
    content = result["result"]["content"]
    assert len(content) > 0, "list_apis returned empty content"
    # Контент должен содержать JSON с массивом контрактов
    text = content[0]["text"]
    assert "name" in text.lower() or "[" in text, f"Unexpected content: {text}"


def test_search_apis_returns_filtered_results(mcp_client: httpx.Client, session: str | None) -> None:
    """search_apis с запросом должен вернуть результаты."""
    result = call_tool(mcp_client, session, "search_apis", {"query": "Analytics"})
    assert "result" in result, f"Unexpected response: {result}"
    content = result["result"]["content"]
    assert len(content) > 0, "search_apis returned empty content"


def test_search_apis_empty_query_returns_results(mcp_client: httpx.Client, session: str | None) -> None:
    """search_apis с пустым запросом должен вернуть результаты."""
    result = call_tool(mcp_client, session, "search_apis", {"query": ""})
    assert "result" in result, f"Unexpected response: {result}"
