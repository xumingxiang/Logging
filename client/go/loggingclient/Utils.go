package loggingclient

import (
	"net"
	"strconv"
	"strings"
)

func getInternalIP() string {

	addrs, err := net.InterfaceAddrs()
	if err != nil {
		return ""
	}
	var ipAddr string
	for _, a := range addrs {
		if ipnet, ok := a.(*net.IPNet); ok && !ipnet.IP.IsLoopback() {
			if ipnet.IP.To4() != nil {

				ipAddr = ipnet.IP.String()
			}
		}
	}
	return ipAddr
}

func ipToNum(ipAddr string) int64 {

	if ipAddr == "" {
		return 0
	}
	bits := strings.Split(ipAddr, ".")
	len := len(bits)
	b0, b1, b2, b3 := 0, 0, 0, 0
	if len >= 1 {
		b0, _ = strconv.Atoi(bits[0])
	}
	if len >= 2 {
		b1, _ = strconv.Atoi(bits[1])
	}
	if len >= 3 {
		b2, _ = strconv.Atoi(bits[2])
	}
	if len >= 4 {
		b3, _ = strconv.Atoi(bits[3])
	}

	var sum int64

	sum += int64(b0) << 24
	sum += int64(b1) << 16
	sum += int64(b2) << 8
	sum += int64(b3)

	return sum
}
